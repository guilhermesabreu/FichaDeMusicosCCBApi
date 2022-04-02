using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Commands
{
    public class CadastrarPessoaCommandHandler : IRequestHandler<CadastrarPessoaCommand, PessoaViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        private readonly UserManager<User> _userManager;
        public CadastrarPessoaCommandHandler(FichaDeMusicosCCBContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<PessoaViewModel> Handle(CadastrarPessoaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region MapearParametro
                TypeAdapterConfig<CadastrarPessoaCommand, Pessoa>.NewConfig()
                    .Map(dest => dest.User.UserName, src => string.IsNullOrEmpty(src.UserName) ? Utils.NomeParaCredencial(src.Nome) : src.UserName)
                    .Map(dest => dest.User.Password, src => string.IsNullOrEmpty(src.Password) ? Utils.NomeParaCredencial(src.Nome) : src.Password)
                    .Map(dest => dest.NomePessoa, src => src.Nome)
                    .Map(dest => dest.ApelidoInstrutorPessoa, src => src.Instrutor)
                    .Map(dest => dest.ApelidoEncarregadoPessoa, src => ObterApelidoPeloNomeCompleto(src.EncarregadoLocal).Result)
                    .Map(dest => dest.ApelidoEncRegionalPessoa, src => ObterApelidoPeloNomeCompleto(src.EncarregadoRegional).Result)
                    .Map(dest => dest.RegiaoPessoa, src => src.Regiao)
                    .Map(dest => dest.RegionalPessoa, src => src.Regional)
                    .Map(dest => dest.CelularPessoa, src => src.Celular)
                    .Map(dest => dest.EmailPessoa, src => src.Email)
                    .Map(dest => dest.DataNascimentoPessoa, src => DateTimeOffset.Parse(src.DataNascimento).UtcDateTime.ToString("dd/MM/yyyy"))
                    .Map(dest => dest.DataInicioPessoa, src => DateTimeOffset.Parse(src.DataInicio).UtcDateTime.ToString("dd/MM/yyyy"))
                    .Map(dest => dest.ComumPessoa, src => src.Comum)
                    .Map(dest => dest.InstrumentoPessoa, src => src.Instrumento)
                    .Map(dest => dest.CondicaoPessoa, src => src.Condicao);
                #endregion
                var pessoaEntity = request.Adapt<Pessoa>();

                await CriarRoles();
                await VerificaExistenciaPessoa(pessoaEntity);
                #region Mapear Response
                TypeAdapterConfig<Pessoa, PessoaViewModel>.NewConfig()
                    .Map(dest => dest.Nome, src => src.NomePessoa)
                    .Map(dest => dest.ApelidoInstrutor, src => src.ApelidoInstrutorPessoa)
                    .Map(dest => dest.ApelidoEncarregado, src => src.ApelidoEncarregadoPessoa)
                    .Map(dest => dest.ApelidoEncRegional, src => src.ApelidoEncRegionalPessoa)
                    .Map(dest => dest.Regiao, src => src.RegiaoPessoa)
                    .Map(dest => dest.Regional, src => src.RegionalPessoa)
                    .Map(dest => dest.Celular, src => src.CelularPessoa)
                    .Map(dest => dest.Email, src => src.EmailPessoa)
                    .Map(dest => dest.DataNascimento, src => src.DataNascimentoPessoa)
                    .Map(dest => dest.DataInicio, src => src.DataInicioPessoa)
                    .Map(dest => dest.Comum, src => src.ComumPessoa)
                    .Map(dest => dest.Instrumento, src => src.InstrumentoPessoa)
                    .Map(dest => dest.Condicao, src => src.CondicaoPessoa);
                #endregion

                var pessoaResponse = await PessoaCriada(pessoaEntity);
                return pessoaResponse.Adapt<PessoaViewModel>();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(Utils.MensagemErro500Padrao);
            }

        }

        public async Task<string> ObterApelidoPeloNomeCompleto(string nomeCompleto)
        {
            var pessoa = await _context.Pessoas.AsNoTracking().Include(x => x.User).Where(x => x.NomePessoa.Equals(nomeCompleto)).FirstOrDefaultAsync();
            if (pessoa == null)
                return string.Empty;

            return pessoa.User.UserName;
        }

        public async Task<Pessoa> PessoaCriada(Pessoa pessoa)
        {
            pessoa.User.Role = pessoa.CondicaoPessoa.ToUpper();
            var result = await _userManager.CreateAsync(pessoa.User, pessoa.User.Password);
            var resultRole = await _userManager.AddToRoleAsync(pessoa.User, pessoa.User.Role);
            if (!resultRole.Succeeded)
                throw new ArgumentException("Não foi possível criar a credencial desta pessoa");

            _context.Pessoas.Add(pessoa);
            if (_context.SaveChanges().Equals(0) || !result.Succeeded)
                throw new ArgumentException("Não foi possível criar esta pessoa, verifique os dados inseridos");

            return pessoa;

        }


        public async Task VerificaExistenciaPessoa(Pessoa pessoa)
        {

            var usuario = await _context.Pessoas.Where(x => x.NomePessoa == pessoa.NomePessoa
                                                    || x.EmailPessoa == pessoa.EmailPessoa
                                                    || x.CelularPessoa == pessoa.CelularPessoa).ToListAsync();
            if (usuario.Count > 0)
                throw new ArgumentException("Os dados desta pessoa já está cadastrado em outra pessoa");

            var credencial = await _context.Users.Where(x => x.UserName == pessoa.User.UserName).ToListAsync();
            if (credencial.Count > 0)
                throw new ArgumentException("Esta pessoa já está cadastrada");

        }

        public async Task CriarRoles()
        {
            var roles = await _context.Roles.Select(x => x.Name).ToListAsync();
            lock (_context)
            {

                if (!roles.Any(x => x == "ENCARREGADO"))
                {
                    Role role = new Role();
                    role.Name = "ENCARREGADO";
                    role.NormalizedName = "ENCARREGADO";
                    _context.Roles.Add(role);
                    _context.SaveChangesAsync();
                }
                if (!roles.Any(x => x == "REGIONAL"))
                {
                    Role role = new Role();
                    role.Name = "REGIONAL";
                    role.NormalizedName = "REGIONAL";
                    _context.Roles.Add(role);
                    _context.SaveChangesAsync();
                }
                if (!roles.Any(x => x == "INSTRUTOR"))
                {
                    Role role = new Role();
                    role.Name = "INSTRUTOR";
                    role.NormalizedName = "INSTRUTOR";
                    _context.Roles.Add(role);
                    _context.SaveChangesAsync();
                }
                if (!roles.Any(x => x == "ALUNO"))
                {
                    Role role = new Role();
                    role.Name = "ALUNO";
                    role.NormalizedName = "ALUNO";
                    _context.Roles.Add(role);
                    _context.SaveChangesAsync();
                }
            }

        }
    }
}

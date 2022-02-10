using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Alunos.Commands
{
    public class CadastrarAlunoCommandHandler : IRequestHandler<CadastrarAlunoCommand, AlunoViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        private readonly UserManager<User> _userManager;
        public CadastrarAlunoCommandHandler(FichaDeMusicosCCBContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<AlunoViewModel> Handle(CadastrarAlunoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region MapearParametro
                TypeAdapterConfig<CadastrarAlunoCommand, Pessoa>.NewConfig()
                    .Map(dest => dest.User.UserName, src => Utils.NomeParaCredencial(src.Nome))
                    .Map(dest => dest.User.Password, src => Utils.NomeParaCredencial(src.Nome))
                    .Map(dest => dest.NomePessoa, src => src.Nome)
                    .Map(dest => dest.ApelidoInstrutorPessoa, src => src.Instrutor)
                    .Map(dest => dest.ApelidoEncarregadoPessoa, src => ObterApelidoPeloNomeCompleto(src.EncarregadoLocal).Result)
                    .Map(dest => dest.ApelidoEncRegionalPessoa, src => ObterApelidoPeloNomeCompleto(src.EncarregadoRegional).Result)
                    .Map(dest => dest.RegiaoPessoa, src => src.Regiao)
                    .Map(dest => dest.RegionalPessoa, src => src.Regional)
                    .Map(dest => dest.CelularPessoa, src => src.Celular)
                    .Map(dest => dest.EmailPessoa, src => src.Email)
                    .Map(dest => dest.DataNascimentoPessoa, src => src.DataNascimento)
                    .Map(dest => dest.DataInicioPessoa, src => src.DataInicio)
                    .Map(dest => dest.ComumPessoa, src => src.Comum)
                    .Map(dest => dest.InstrumentoPessoa, src => src.Instrumento)
                    .Map(dest => dest.CondicaoPessoa, src => src.Condicao);
                #endregion
                var pessoaEntity = request.Adapt<Pessoa>();

                await VerificaExistenciaAluno(pessoaEntity);
                #region Mapear Response
                TypeAdapterConfig<Pessoa, AlunoViewModel>.NewConfig()
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

                var pessoaResponse = await AlunoCriado(pessoaEntity);
                return pessoaResponse.Adapt<AlunoViewModel>();
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
            var pessoa = _context.Pessoas.AsNoTracking().Include(x => x.User).Where(x => x.NomePessoa.Equals(nomeCompleto)).FirstOrDefaultAsync();
            if (pessoa.Result == null)
                throw new ArgumentException("Encarregado inexistente");

            return pessoa.Result.User.UserName;
        }

        public async Task<Pessoa> AlunoCriado(Pessoa pessoa)
        {
            pessoa.User.Role = pessoa.CondicaoPessoa.ToUpper();
            var result = await _userManager.CreateAsync(pessoa.User, pessoa.User.Password);
            var resultRole = await _userManager.AddToRoleAsync(pessoa.User, pessoa.User.Role);
            if (!resultRole.Succeeded)
                throw new ArgumentException("Não foi possível criar a credencial deste aluno");

            _context.Pessoas.Add(pessoa);
            if (_context.SaveChanges().Equals(0) || !result.Succeeded)
                throw new ArgumentException("Não foi possível criar este aluno, verifique os dados inseridos");

            return pessoa;

        }

        public async Task VerificaExistenciaAluno(Pessoa pessoa)
        {
            var query = await _context.Pessoas.Where(x => x.NomePessoa == pessoa.NomePessoa
                                                    || x.EmailPessoa == pessoa.EmailPessoa
                                                    || x.CelularPessoa == pessoa.CelularPessoa).ToListAsync();

            if (query.Count > 0)
                throw new ArgumentException("Os dados deste aluno já está cadastrado em outra pessoa");

        }
    }
}

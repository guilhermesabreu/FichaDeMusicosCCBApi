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
    public class AtualizarPessoaCommandHandler : IRequestHandler<AtualizarPessoaCommand, PessoaViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AtualizarPessoaCommandHandler(FichaDeMusicosCCBContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<PessoaViewModel> Handle(AtualizarPessoaCommand request, CancellationToken cancellationToken)
        {
            try
            {

                #region MapearParametro
                TypeAdapterConfig<AtualizarPessoaCommand, Pessoa>.NewConfig()
                    .Map(dest => dest.IdPessoa, src => src.Id)
                    .Map(dest => dest.User.UserName, src => src.UserName)
                    .Map(dest => dest.User.Password, src => src.Password)
                    .Map(dest => dest.NomePessoa, src => src.Nome)
                    .Map(dest => dest.ApelidoInstrutorPessoa, src => ObterApelidoPeloNomeCompleto(src.Instrutor).Result)
                    .Map(dest => dest.ApelidoEncarregadoPessoa, src => ObterApelidoPeloNomeCompleto(src.EncarregadoLocal).Result)
                    .Map(dest => dest.ApelidoEncRegionalPessoa, src => ObterApelidoPeloNomeCompleto(src.EncarregadoRegional).Result)
                    .Map(dest => dest.RegiaoPessoa, src => src.Regiao)
                    .Map(dest => dest.RegionalPessoa, src => src.Regional)
                    .Map(dest => dest.CelularPessoa, src => src.Celular)
                    .Map(dest => dest.EmailPessoa, src => src.Email)
                    .Map(dest => dest.DataNascimentoPessoa, src => DateTimeOffset.Parse(src.DataNascimento).UtcDateTime)
                    .Map(dest => dest.DataInicioPessoa, src => DateTimeOffset.Parse(src.DataInicio).UtcDateTime)
                    .Map(dest => dest.ComumPessoa, src => src.Comum)
                    .Map(dest => dest.InstrumentoPessoa, src => src.Instrumento)
                    .Map(dest => dest.CondicaoPessoa, src => src.Condicao);
                #endregion
                var pessoaAtual = request.Adapt<Pessoa>();
                await VerificaDadosObrigatorios(pessoaAtual);
                await VerificaMinisterioNaBase(pessoaAtual);

                var pessoaAntiga = await PessoaEncontrada(pessoaAtual.IdPessoa);
                #region Mapear Response
                TypeAdapterConfig<FichaDeMusicosCCB.Domain.Entities.Pessoa, PessoaViewModel>.NewConfig()
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

                var pessoaResponse = await PessoaAtualizada(pessoaAntiga, pessoaAtual);
                return pessoaResponse.Adapt<PessoaViewModel>();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> ObterApelidoPeloNomeCompleto(string nomeCompleto)
        {
            var pessoa = await _context.Pessoas.AsNoTracking().Include(x => x.User).Where(x => x.NomePessoa.Equals(nomeCompleto)).FirstOrDefaultAsync();
            if (pessoa == null)
                return !string.IsNullOrEmpty(nomeCompleto) ? nomeCompleto : "";

            return pessoa.User.UserName;
        }

        public async Task VerificaDadosObrigatorios(Pessoa pessoa)
        {
            switch (pessoa.CondicaoPessoa.ToUpper())
            {
                case "ENCARREGADO":
                    if (string.IsNullOrEmpty(pessoa.ApelidoEncRegionalPessoa)
                    || string.IsNullOrEmpty(pessoa.ComumPessoa))
                        throw new ArgumentException("Encarregado Regional ou comum não foram preenchido."); break;

                case "INSTRUTOR":
                    if (string.IsNullOrEmpty(pessoa.ApelidoEncRegionalPessoa)
                        || string.IsNullOrEmpty(pessoa.ApelidoEncarregadoPessoa)
                        || string.IsNullOrEmpty(pessoa.ComumPessoa))
                        throw new ArgumentException("Encarregado Regional, Encarregado local ou comum não foram preenchidos."); break;

            }
            if (string.IsNullOrEmpty(pessoa.ComumPessoa) || string.IsNullOrEmpty(pessoa.RegiaoPessoa)
                || string.IsNullOrEmpty(pessoa.RegionalPessoa))
                throw new ArgumentException("Dados de localidade não informados");
        }

        public async Task VerificaMinisterioNaBase(Pessoa pessoa)
        {
            switch (pessoa.CondicaoPessoa.ToUpper())
            {
                case "ENCARREGADO":
                    if (!string.IsNullOrEmpty(pessoa.ApelidoEncRegionalPessoa))
                    {
                        if (!_context.Users.AsNoTracking().Any(x => x.UserName.Equals(pessoa.ApelidoEncRegionalPessoa)))
                            throw new ArgumentException("O Encarregado Regional não existe"); break;
                    }
                    break;

                case "INSTRUTOR":
                    if (!string.IsNullOrEmpty(pessoa.ApelidoEncRegionalPessoa) || !string.IsNullOrEmpty(pessoa.ApelidoEncarregadoPessoa))
                    {
                        if (!_context.Users.AsNoTracking().Any(x => x.UserName.Equals(pessoa.ApelidoEncRegionalPessoa))
                          || !_context.Users.AsNoTracking().Any(x => x.UserName.Equals(pessoa.ApelidoEncarregadoPessoa)))
                            throw new ArgumentException("O Encarregado Regional ou Encarregado Local não existem"); break;
                    }
                    break;

            }
        }

        public async Task<Pessoa> PessoaAtualizada(Pessoa pessoaAntiga, Pessoa pessoaAtual)
        {

            var encarregadoLocalExistente = _context.Pessoas.AsNoTracking()
                .Where(x => x.ComumPessoa.Equals(pessoaAtual.ComumPessoa)
                && x.RegiaoPessoa.Equals(pessoaAtual.RegiaoPessoa)
                && x.RegionalPessoa.Equals(pessoaAtual.RegionalPessoa)
                && pessoaAtual.CondicaoPessoa.ToUpper().Equals("ENCARREGADO")
                && x.CondicaoPessoa.ToUpper().Equals("ENCARREGADO")).FirstOrDefault();

            if (encarregadoLocalExistente != null)
                throw new ArgumentException("Já existe um encarregado Local nesta comum congregação");

            var userNameAtual = pessoaAtual.User.UserName == null ? pessoaAntiga.User.UserName : pessoaAtual.User.UserName;
            var senhaAtual = pessoaAtual.User.Password == null ? pessoaAntiga.User.Password : pessoaAtual.User.Password;

            var senhaAntiga = pessoaAntiga.User.Password;
            pessoaAntiga.ApelidoInstrutorPessoa = string.IsNullOrEmpty(pessoaAntiga.ApelidoInstrutorPessoa) 
                                                ? "" : pessoaAntiga.ApelidoInstrutorPessoa;

            var pessoaAtualizada = pessoaAtual;
            pessoaAtualizada.User = pessoaAntiga.User;
            pessoaAtualizada.User.NormalizedUserName = userNameAtual;
            pessoaAtualizada.User.UserName = userNameAtual;
            pessoaAtualizada.User.Password = senhaAtual;
            pessoaAtualizada.ApelidoInstrutorPessoa = !pessoaAntiga.ApelidoInstrutorPessoa.Contains(pessoaAtual.ApelidoInstrutorPessoa) 
                                                    ? pessoaAntiga.ApelidoInstrutorPessoa +";"+ pessoaAtual.ApelidoInstrutorPessoa
                                                    : pessoaAntiga.ApelidoInstrutorPessoa;

            _context.Pessoas.Update(pessoaAtualizada);
            _context.SaveChanges();

            //Realizar atualização do UserName e Password corretamente
            var result = await _userManager.UpdateAsync(pessoaAtual.User);
            if (!result.Succeeded)
                throw new ArgumentException("Não foi possível atualizar a credencial desta pessoa");

            var atualizaSenha = await _userManager.ChangePasswordAsync(pessoaAntiga.User, senhaAntiga, senhaAtual);
            if (!atualizaSenha.Succeeded)
                throw new ArgumentException("Não foi possível atualizar a credencial desta pessoa");

            return pessoaAtual;
        }

        public async Task<Pessoa> PessoaEncontrada(long idPessoa)
        {
            var query = await _context.Pessoas.AsNoTracking().Include(x => x.User).Where(x => x.IdPessoa == idPessoa).FirstOrDefaultAsync();
            if (query == null)
                throw new ArgumentException("Usuário não encontrado");

            return query;
        }

    }
}

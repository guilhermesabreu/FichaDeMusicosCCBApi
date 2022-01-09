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
    public class AtualizarAlunoCommandHandler : IRequestHandler<AtualizarAlunoCommand, AlunoViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        private readonly UserManager<User> _userManager;
        public AtualizarAlunoCommandHandler(FichaDeMusicosCCBContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<AlunoViewModel> Handle(AtualizarAlunoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region MapearParametro
                TypeAdapterConfig<AtualizarAlunoCommand, Pessoa>.NewConfig()
                    .Map(dest => dest.IdPessoa, src => src.Id)
                    .Map(dest => dest.User.UserName, src => Utils.NomeParaCredencial(src.Nome))
                    .Map(dest => dest.User.Password, src => Utils.NomeParaCredencial(src.Nome))
                    .Map(dest => dest.NomePessoa, src => src.Nome)
                    .Map(dest => dest.ApelidoInstrutorPessoa, src => src.ApelidoInstrutor)
                    .Map(dest => dest.ApelidoEncarregadoPessoa, src => src.ApelidoEncarregado)
                    .Map(dest => dest.ApelidoEncRegionalPessoa, src => src.ApelidoEncRegional)
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
                var alunoAtual = request.Adapt<Pessoa>();

                var alunoAntigo = await AlunoEncontrado(alunoAtual.IdPessoa);
                #region Mapear Response
                TypeAdapterConfig<Pessoa, AlunoViewModel>.NewConfig()
                    .Map(dest => dest.Id, src => src.IdPessoa)
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

                var pessoaResponse = await AlunoAtualizado(alunoAntigo, alunoAtual);
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

        public async Task<Pessoa> AlunoAtualizado(Pessoa pessoaAntiga, Pessoa pessoaAtual)
        {
            var userNameAtual = pessoaAtual.User.UserName;
            var senhaAtual = pessoaAtual.User.Password;
            var senhaAntiga = pessoaAntiga.User.Password;

            var pessoaAtualizada = pessoaAtual;
            pessoaAtualizada.User = pessoaAntiga.User;
            pessoaAtualizada.User.NormalizedUserName = userNameAtual;
            pessoaAtualizada.User.UserName = userNameAtual;
            pessoaAtualizada.User.Password = senhaAtual;

            _context.Pessoas.Update(pessoaAtualizada);
            if (_context.SaveChanges().Equals(0))
                throw new ArgumentException("Não foi possível atualizar esta pessoa, verifique os dados inseridos");

            //Realizar atualização do UserName e Password corretamente
            var result = await _userManager.UpdateAsync(pessoaAtual.User);
            if (!result.Succeeded)
                throw new ArgumentException("Não foi possível atualizar a credencial deste aluno");

            var atualizaSenha = await _userManager.ChangePasswordAsync(pessoaAntiga.User, senhaAntiga, senhaAtual);
            if (!atualizaSenha.Succeeded)
                throw new ArgumentException("Não foi possível atualizar a credencial deste aluno");

            return pessoaAtual;
        }

        public async Task<Pessoa> AlunoEncontrado(long idPessoa)
        {
            var query = await _context.Pessoas.AsNoTracking().Include(x => x.User).Where(x => x.IdPessoa == idPessoa).FirstOrDefaultAsync();
            if (query == null)
                throw new ArgumentException("Aluno não encontrado");

            if (!query.CondicaoPessoa.ToUpper().Equals("ALUNO"))
                throw new ArgumentException("Esta pessoa não está na condição de aluno");

            return query;
        }
    }
}

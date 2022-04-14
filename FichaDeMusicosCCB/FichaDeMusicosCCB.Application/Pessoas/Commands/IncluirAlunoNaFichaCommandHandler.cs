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
    public class IncluirAlunoNaFichaCommandHandler : IRequestHandler<IncluirAlunoNaFichaCommand, PessoaViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public IncluirAlunoNaFichaCommandHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<PessoaViewModel> Handle(IncluirAlunoNaFichaCommand request, CancellationToken cancellationToken)
        {
            try
            {
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
                var pessoaResponse = await InserirAlunoNaFicha(request);
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

        public async Task<Pessoa> InserirAlunoNaFicha(IncluirAlunoNaFichaCommand request)
        {
            var pessoaEncontrada = new Pessoa();
            var pessoaFicha = _context.Pessoas.AsNoTracking().Include(x => x.User).Where(x => x.User.UserName.Equals(request.ApelidoDonoDaFicha)).FirstOrDefault();
            if (pessoaFicha == null)
                throw new ArgumentException("Você não foi encontrado na base de dados");

            var alunoEncontrado = await _context.Pessoas.AsNoTracking().Where(x => x.NomePessoa.Equals(request.NomeAluno)).FirstOrDefaultAsync();
            if (alunoEncontrado == null)
                throw new ArgumentException("Aluno não encontrado");


            alunoEncontrado.ApelidoInstrutorPessoa = string.IsNullOrEmpty(alunoEncontrado.ApelidoInstrutorPessoa) ? "" : alunoEncontrado.ApelidoInstrutorPessoa;
            alunoEncontrado.ApelidoEncarregadoPessoa = string.IsNullOrEmpty(alunoEncontrado.ApelidoEncarregadoPessoa) ? "" : alunoEncontrado.ApelidoEncarregadoPessoa;
            alunoEncontrado.ApelidoEncRegionalPessoa = string.IsNullOrEmpty(alunoEncontrado.ApelidoEncRegionalPessoa) ? "" : alunoEncontrado.ApelidoEncRegionalPessoa;
            switch (pessoaFicha.CondicaoPessoa.ToUpper())
            {
                case "INSTRUTOR":

                    if (alunoEncontrado.ApelidoInstrutorPessoa.Contains(pessoaFicha.User.UserName))
                        throw new ArgumentException("Você já inseriu este aluno anteriormente");

                    alunoEncontrado.ApelidoInstrutorPessoa = !alunoEncontrado.ApelidoInstrutorPessoa.Contains(pessoaFicha.User.UserName)
                                                    ? alunoEncontrado.ApelidoInstrutorPessoa + ";" + pessoaFicha.User.UserName
                                                    : alunoEncontrado.ApelidoInstrutorPessoa;
                    break;
                case "REGIONAL":
                    if (string.IsNullOrEmpty(alunoEncontrado.ApelidoEncRegionalPessoa))
                    {
                        alunoEncontrado.ApelidoEncRegionalPessoa = pessoaFicha.User.UserName;break;
                    }
                    throw new ArgumentException("Este Aluno já possui um Encarregado Regional");
                case "ENCARREGADO":
                    if (string.IsNullOrEmpty(alunoEncontrado.ApelidoEncarregadoPessoa))
                    {
                        alunoEncontrado.ApelidoEncarregadoPessoa = pessoaFicha.User.UserName;break;
                    }
                    throw new ArgumentException("Este Aluno já possui um Encarregado Local");
            }

            _context.Pessoas.Update(alunoEncontrado);
            _context.SaveChanges();

            return alunoEncontrado;
        }

    }
}

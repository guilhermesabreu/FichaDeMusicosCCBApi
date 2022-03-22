using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarPessoaLogadaQueryHandler : IRequestHandler<BuscarPessoaLogadaQuery, PessoaViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public BuscarPessoaLogadaQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<PessoaViewModel> Handle(BuscarPessoaLogadaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                #region Mapeamento
                TypeAdapterConfig<Pessoa, PessoaViewModel>.NewConfig()
                        .Map(dest => dest.Id, src => src.IdPessoa)
                        .Map(dest => dest.Nome, src => src.NomePessoa)
                        .Map(dest => dest.ApelidoInstrutor, src => src.ApelidoInstrutorPessoa)
                        .Map(dest => dest.ApelidoEncarregado, src => src.ApelidoEncarregadoPessoa)
                        .Map(dest => dest.ApelidoEncRegional, src => src.ApelidoEncRegionalPessoa)
                        .Map(dest => dest.Regiao, src => src.RegiaoPessoa)
                        .Map(dest => dest.Regional, src => src.RegionalPessoa)
                        .Map(dest => dest.Celular, src => src.CelularPessoa)
                        .Map(dest => dest.Email, src => src.EmailPessoa)
                        .Map(dest => dest.DataNascimento, src => Utils.DataString(src.DataNascimentoPessoa))
                        .Map(dest => dest.DataInicio, src => Utils.DataString(src.DataInicioPessoa))
                        .Map(dest => dest.Comum, src => src.ComumPessoa)
                        .Map(dest => dest.Instrumento, src => src.InstrumentoPessoa)
                        .Map(dest => dest.Condicao, src => src.CondicaoPessoa);
                #endregion

                var pessoas = _context.Pessoas.AsQueryable().Include(x => x.User);
                var pessoaFiltrada = pessoas.Where(x => x.User.UserName.Equals(request.Apelido)).FirstOrDefault();
                if (pessoaFiltrada is null)
                    throw new ArgumentException("Pessoa não encontrada");

                return pessoaFiltrada.Adapt<PessoaViewModel>();
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

    }
}

using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Queries
{
    public class BuscarEncarregadoLocalQueryHandler : IRequestHandler<BuscarEncarregadoLocalQuery, List<PessoaViewModel>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public BuscarEncarregadoLocalQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<PessoaViewModel>> Handle(BuscarEncarregadoLocalQuery request, CancellationToken cancellationToken)
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

                if (string.IsNullOrEmpty(request.ApelidoPessoaLogada))
                    return _context.Pessoas.AsNoTracking().Include(x => x.User)
                        .Where(x => x.NomePessoa.StartsWith(request.Input)
                        && x.User.Role.Equals("ENCARREGADO")).ToList().Adapt<List<PessoaViewModel>>();

                var pessoaLogada = PessoaLogada(request).Result;
                return _context.Pessoas.AsNoTracking().Include(x => x.User)
                    .Where(x => x.NomePessoa.StartsWith(request.Input)
                    && x.User.Role.Equals("ENCARREGADO")
                    && x.RegiaoPessoa.Equals(pessoaLogada.RegiaoPessoa)
                    && x.RegionalPessoa.Equals(pessoaLogada.RegionalPessoa)).ToList().Adapt<List<PessoaViewModel>>();

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

        public async Task<Pessoa> PessoaLogada(BuscarEncarregadoLocalQuery query)
        {
            return _context.Pessoas.AsNoTracking().Include(x => x.User)
                .Where(x => x.User.UserName.Equals(query.ApelidoPessoaLogada)).FirstOrDefault();
        }

    }
}

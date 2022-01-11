using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class ConsultarPessoasPorApelidoECondicaoQueryHandler : IRequestHandler<ConsultarPessoasPorApelidoECondicaoQuery, List<PessoaViewModel>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public ConsultarPessoasPorApelidoECondicaoQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<PessoaViewModel>> Handle(ConsultarPessoasPorApelidoECondicaoQuery request, CancellationToken cancellationToken)
        {
            #region Mapeamento
            TypeAdapterConfig<Pessoa, PessoaViewModel>.NewConfig()
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

            TypeAdapterConfig<Ocorrencia, OcorrenciaViewModel>.NewConfig()
                    .Map(dest => dest.DataOcorrencia, src => Utils.DataString(src.DataOcorrencia))        
                    .Map(dest => dest.NumeroLicao, src => src.NumeroLicaoOcorrencia)
                    .Map(dest => dest.NomeMetodo, src => src.MetodoOcorrencia)
                    .Map(dest => dest.ObservacaoInstrutor, src => src.ObservacaoOcorrencia);

            //Observar se irá trazer os dados da ocorrência e hinos
            #endregion
            var pessoas = _context.Pessoas.AsQueryable();
            var pessoasPorInstrutor = pessoas.Where(x => (x.ApelidoInstrutorPessoa.Equals(request.ApelidoInstrutor)
                                                      || x.ApelidoEncarregadoPessoa.Equals(request.ApelidoEncarregado)
                                                      || x.ApelidoEncRegionalPessoa.Equals(request.ApelidoEncarregadoRegional))
                                                      && (x.CondicaoPessoa.Equals(request.Condicao)));
            return pessoasPorInstrutor.Adapt<List<PessoaViewModel>>();
        }

    }
}

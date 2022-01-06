using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class ConsultarPessoasPorInstrutorQueryHandler : IRequestHandler<ConsultarPessoasPorInstrutorQuery, List<PessoaViewModel>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public ConsultarPessoasPorInstrutorQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<PessoaViewModel>> Handle(ConsultarPessoasPorInstrutorQuery request, CancellationToken cancellationToken)
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
                    .Map(dest => dest.DataNascimento, src => src.DataNascimentoPessoa)
                    .Map(dest => dest.DataInicio, src => src.DataInicioPessoa)
                    .Map(dest => dest.Comum, src => src.ComumPessoa)
                    .Map(dest => dest.Instrumento, src => src.InstrumentoPessoa)
                    .Map(dest => dest.Condicao, src => src.CondicaoPessoa)
                    .Map(dest => dest.PessoaOcorrencias, src => src.PessoaOcorrencias);
            //Observar se irá trazer os dados da ocorrência e hinos
            #endregion
            var pessoas = _context.Pessoas.AsQueryable();
            var pessoasPorInstrutor = pessoas.Where(x => x.ApelidoInstrutorPessoa.Equals(request.ApelidoInstrutor));
            return pessoasPorInstrutor.Adapt<List<PessoaViewModel>>();
        }

    }
}

using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class ConsultarPessoasPorEncarregadoQueryHandler : IRequestHandler<ConsultarPessoasPorEncarregadoQuery, List<PessoaViewModel>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public ConsultarPessoasPorEncarregadoQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<PessoaViewModel>> Handle(ConsultarPessoasPorEncarregadoQuery request, CancellationToken cancellationToken)
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

            var pessoas = await _context.Pessoas.AsQueryable().Include(x => x.PessoaOcorrencias).ToListAsync();
            var pessoasPorEncarregado = pessoas.Where(x => x.ApelidoEncarregadoPessoa.Equals(request.ApelidoEncarregado));
            return pessoasPorEncarregado.Adapt<List<PessoaViewModel>>();
        }

    }
}

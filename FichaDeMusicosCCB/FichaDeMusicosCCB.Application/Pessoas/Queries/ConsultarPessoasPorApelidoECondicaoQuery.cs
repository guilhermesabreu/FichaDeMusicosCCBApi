using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class ConsultarPessoasPorApelidoECondicaoQuery : IRequest<List<PessoaViewModel>>
    {
        public string? ApelidoInstrutor { get; set; }
        public string? ApelidoEncarregado { get; set; }
        public string? ApelidoEncarregadoRegional { get; set; }
        public string? Condicao { get; set; }
        public ConsultarPessoasPorApelidoECondicaoQuery(PessoaQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

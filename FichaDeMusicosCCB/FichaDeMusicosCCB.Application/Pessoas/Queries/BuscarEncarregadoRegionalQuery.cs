using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarEncarregadoRegionalQuery : IRequest<List<string>>
    {
        public string? Input { get; set; }
        public string? ApelidoPessoaLogada { get; set; }
        public BuscarEncarregadoRegionalQuery(BuscaPessoasQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

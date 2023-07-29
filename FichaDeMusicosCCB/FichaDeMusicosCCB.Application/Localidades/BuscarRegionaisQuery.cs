using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Localidades.Query
{
    public class BuscarRegionaisQuery : IRequest<List<string>>
    {
        public string? Input { get; set; }
        public string? ApelidoPessoaLogada { get; set; }
        public BuscarRegionaisQuery(BuscaLocalidadesQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

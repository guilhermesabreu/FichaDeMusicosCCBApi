using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Localidades.Query
{
    public class BuscarRegioesQuery : IRequest<List<string>>
    {
        public string? Input { get; set; }
        public string? ApelidoPessoaLogada { get; set; }
        public BuscarRegioesQuery(BuscaLocalidadesQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

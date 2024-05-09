using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Queries
{
    public class BuscarEncarregadoLocalQuery : IRequest<List<PessoaViewModel>>
    {
        public string? Input { get; set; }
        public string? ApelidoPessoaLogada { get; set; }
        public BuscarEncarregadoLocalQuery(BuscaPessoasQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

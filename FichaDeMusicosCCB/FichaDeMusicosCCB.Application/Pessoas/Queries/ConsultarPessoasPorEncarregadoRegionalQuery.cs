using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class ConsultarPessoasPorEncarregadoRegionalQuery : IRequest<List<PessoaViewModel>>
    {
        public string ApelidoEncarregadoRegional { get; set; }
        public ConsultarPessoasPorEncarregadoRegionalQuery(PessoaQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

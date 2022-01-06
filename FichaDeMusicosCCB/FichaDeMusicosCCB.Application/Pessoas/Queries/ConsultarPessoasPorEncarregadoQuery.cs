using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class ConsultarPessoasPorEncarregadoQuery : IRequest<List<PessoaViewModel>>
    {
        public string ApelidoEncarregado { get; set; }
        public ConsultarPessoasPorEncarregadoQuery(PessoaQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

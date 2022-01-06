using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class ConsultarPessoasPorInstrutorQuery : IRequest<List<PessoaViewModel>>
    {
        public string ApelidoInstrutor { get; set; }
        public ConsultarPessoasPorInstrutorQuery(PessoaQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

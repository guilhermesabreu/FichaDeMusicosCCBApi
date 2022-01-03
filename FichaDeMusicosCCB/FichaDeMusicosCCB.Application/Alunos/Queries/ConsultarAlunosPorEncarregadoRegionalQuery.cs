using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Alunos.Query
{
    public class ConsultarAlunosPorEncarregadoRegionalQuery : IRequest<List<PessoaViewModel>>
    {
        public string ApelidoEncarregadoRegional { get; set; }
        public ConsultarAlunosPorEncarregadoRegionalQuery(AlunoQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

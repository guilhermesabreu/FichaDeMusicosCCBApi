using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Alunos.Query
{
    public class ConsultarAlunosPorInstrutorQuery : IRequest<List<PessoaViewModel>>
    {
        public string ApelidoInstrutor { get; set; }
        public ConsultarAlunosPorInstrutorQuery(AlunoQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

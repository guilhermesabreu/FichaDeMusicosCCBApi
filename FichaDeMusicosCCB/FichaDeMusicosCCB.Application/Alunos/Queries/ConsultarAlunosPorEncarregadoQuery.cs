using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Alunos.Query
{
    public class ConsultarAlunosPorEncarregadoQuery : IRequest<List<PessoaViewModel>>
    {
        public string ApelidoEncarregado { get; set; }
        public ConsultarAlunosPorEncarregadoQuery(AlunoQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

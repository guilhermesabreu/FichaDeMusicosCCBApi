using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Alunos.Commands
{
    public class ExcluirAlunoCommand : IRequest<bool>
    {
        public long IdAluno { get; set; }
        public ExcluirAlunoCommand(long idAluno)
        {
            IdAluno = idAluno;
        }
    }
}

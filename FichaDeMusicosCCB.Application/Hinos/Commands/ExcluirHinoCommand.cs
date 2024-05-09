using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Hinos.Commands
{
    public class ExcluirHinoCommand : IRequest<bool>
    {
        public long IdHino { get; set; }
        public ExcluirHinoCommand(long idHino)
        {
            IdHino = idHino;
        }
    }
}

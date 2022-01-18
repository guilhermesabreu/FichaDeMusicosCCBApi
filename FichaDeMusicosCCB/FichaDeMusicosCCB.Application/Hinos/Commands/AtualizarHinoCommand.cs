using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Hinos.Commands
{
    public class AtualizarHinoCommand : IRequest<HinoViewModel>
    {
        public int IdHino { get; set; }
        public int Numero { get; set; }
        public string? Voz { get; set; }
        public int IdPessoa { get; set; }
        public AtualizarHinoCommand(HinoInputModel input)
        {
            input.Adapt(this);
        }
    }
}

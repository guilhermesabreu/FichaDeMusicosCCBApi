using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Hinos.Commands
{
    public class CadastrarHinoCommand : IRequest<HinoViewModel>
    {
        public int Numero { get; set; }
        public string? Voz { get; set; }
        public string? Data { get; set; }
        public int IdPessoa { get; set; }
        public CadastrarHinoCommand(HinoInputModel input)
        {
            input.Adapt(this);
        }
    }
}

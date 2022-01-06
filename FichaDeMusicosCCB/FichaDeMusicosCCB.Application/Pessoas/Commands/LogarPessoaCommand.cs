using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Commands
{
    public class LogarPessoaCommand : IRequest<string>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public LogarPessoaCommand(CredencialInputModel input)
        {
            input.Adapt(this);
        }
    }
}

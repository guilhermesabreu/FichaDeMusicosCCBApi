using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Commands
{
    public class ExcluirPessoaCommand : IRequest<bool>
    {
        public string UserName { get; set; }
        public ExcluirPessoaCommand(string userName)
        {
            UserName = userName;
        }
    }
}

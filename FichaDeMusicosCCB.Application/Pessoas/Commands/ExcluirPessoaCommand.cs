using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Commands
{
    public class ExcluirPessoaCommand : IRequest<bool>
    {
        public long IdPessoa { get; set; }
        public ExcluirPessoaCommand(long idPessoa)
        {
            IdPessoa = idPessoa;
        }
    }
}

using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Commands
{
    public class ExcluirPessoaCommand : IRequest<bool>
    {
        public long IdPessoa { get; set; }
        public string? ApelidoDonoDaFicha { get; set; }
        public ExcluirPessoaCommand(ExclusaoPessoaInputModel input)
        {
            input.Adapt(this);
        }
    }
}

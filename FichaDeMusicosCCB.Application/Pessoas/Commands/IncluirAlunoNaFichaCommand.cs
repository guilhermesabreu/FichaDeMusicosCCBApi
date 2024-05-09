using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Commands
{
    public class IncluirAlunoNaFichaCommand : IRequest<PessoaViewModel>
    {
        public string? ApelidoDonoDaFicha { get; set; }
        public string? NomeAluno { get; set; }
        public IncluirAlunoNaFichaCommand(AlunoNaFichaInputModel input)
        {
            input.Adapt(this);
        }
    }
}

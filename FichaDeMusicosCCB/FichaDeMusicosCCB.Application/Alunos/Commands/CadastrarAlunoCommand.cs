using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Alunos.Commands
{
    public class CadastrarAlunoCommand : IRequest<AlunoViewModel>
    {
        public string? Nome { get; set; }
        public string? ApelidoInstrutor { get; set; }
        public string? ApelidoEncarregado { get; set; }
        public string? ApelidoEncRegional { get; set; }
        public string? Regiao { get; set; }
        public string? Regional { get; set; }
        public string? Celular { get; set; }
        public string? Email { get; set; }
        public string? DataNascimento { get; set; }
        public string? DataInicio { get; set; }
        public string? Comum { get; set; }
        public string? Instrumento { get; set; }
        public string? Condicao { get; set; }
        public CadastrarAlunoCommand(AlunoInputModel input)
        {
            input.Adapt(this);
        }
    }
}

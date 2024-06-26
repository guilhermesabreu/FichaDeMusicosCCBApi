﻿using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Commands
{
    public class CadastrarPessoaCommand : IRequest<PessoaViewModel>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Nome { get; set; }
        public string? Instrutor { get; set; }
        public string? EncarregadoLocal { get; set; }
        public string? EncarregadoRegional { get; set; }
        public string? Regiao { get; set; }
        public string? Regional { get; set; }
        public string? Celular { get; set; }
        public string? Email { get; set; }
        public string? DataNascimento { get; set; }
        public string? DataInicio { get; set; }
        public string? Comum { get; set; }
        public string? Instrumento { get; set; }
        public string? Condicao { get; set; }
        public CadastrarPessoaCommand(PessoaInputModel input)
        {
            input.Adapt(this);
        }
    }
}

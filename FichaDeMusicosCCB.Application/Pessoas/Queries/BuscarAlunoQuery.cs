﻿using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Queries
{
    public class BuscarAlunoQuery : IRequest<List<string>>
    {
        public string? Input { get; set; }
        public string? ApelidoPessoaLogada { get; set; }
        public BuscarAlunoQuery(BuscaPessoasQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

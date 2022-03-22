using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarPessoaLogadaQuery : IRequest<PessoaViewModel>
    {
        public string? Apelido { get; set; }
        public BuscarPessoaLogadaQuery(string apelido)
        {
            Apelido = apelido;
        }
    }
}

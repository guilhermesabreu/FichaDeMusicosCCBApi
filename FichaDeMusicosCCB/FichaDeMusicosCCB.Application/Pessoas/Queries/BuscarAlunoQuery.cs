using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarAlunoQuery : IRequest<List<string>>
    {
        public string? Text { get; set; }
        public BuscarAlunoQuery(string text)
        {
            Text = text;
        }
    }
}

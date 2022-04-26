using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarComunsQuery : IRequest<List<string>>
    {
        public string? Text { get; set; }
        public BuscarComunsQuery(string text)
        {
            Text = text;
        }
    }
}

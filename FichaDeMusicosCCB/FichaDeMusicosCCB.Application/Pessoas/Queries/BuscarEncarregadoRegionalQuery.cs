using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarEncarregadoRegionalQuery : IRequest<List<string>>
    {
        public string? Text { get; set; }
        public BuscarEncarregadoRegionalQuery(string text)
        {
            Text = text;
        }
    }
}

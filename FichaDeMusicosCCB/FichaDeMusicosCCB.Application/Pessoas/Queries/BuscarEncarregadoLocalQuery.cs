using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarEncarregadoLocalQuery : IRequest<List<string>>
    {
        public string? Text { get; set; }
        public BuscarEncarregadoLocalQuery(string text)
        {
            Text = text;
        }
    }
}

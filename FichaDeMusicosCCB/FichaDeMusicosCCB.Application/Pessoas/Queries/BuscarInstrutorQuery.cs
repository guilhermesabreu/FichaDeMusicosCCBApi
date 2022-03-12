using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarInstrutorQuery : IRequest<List<string>>
    {
        public string? Text { get; set; }
        public BuscarInstrutorQuery(string text)
        {
            Text = text;
        }
    }
}

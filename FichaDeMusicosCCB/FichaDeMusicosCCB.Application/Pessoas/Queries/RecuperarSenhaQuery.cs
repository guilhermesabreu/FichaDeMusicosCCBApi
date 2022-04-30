using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.QueryParameters;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class RecuperarSenhaQuery : IRequest<bool>
    {
        public string? Email { get; set; }
        public RecuperarSenhaQuery(RecuperaSenhaQueryParameter parameters)
        {
            parameters.Adapt(this);
        }
    }
}

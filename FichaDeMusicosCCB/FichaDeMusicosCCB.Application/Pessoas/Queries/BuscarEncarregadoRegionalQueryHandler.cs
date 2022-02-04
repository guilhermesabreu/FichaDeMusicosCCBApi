using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarEncarregadoRegionalQueryHandler : IRequestHandler<BuscarEncarregadoRegionalQuery, List<string>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public BuscarEncarregadoRegionalQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<string>> Handle(BuscarEncarregadoRegionalQuery request, CancellationToken cancellationToken)
        {
            var pessoas = _context.Pessoas.AsQueryable().Include(x => x.User);
            var encarregados = pessoas.Where(x => x.NomePessoa.StartsWith(request.Text) && x.User.Role.Equals("REGIONAL")).Select(x => x.User.UserName +"-"+x.NomePessoa);
            return encarregados.ToList();
        }

    }
}

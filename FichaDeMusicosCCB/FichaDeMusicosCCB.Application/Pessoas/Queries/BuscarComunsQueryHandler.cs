using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarComunsQueryHandler : IRequestHandler<BuscarComunsQuery, List<string>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public BuscarComunsQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<string>> Handle(BuscarComunsQuery request, CancellationToken cancellationToken)
        {
            var pessoas = _context.Pessoas.AsQueryable().Include(x => x.User);
            var comuns = pessoas.Where(x => x.ApelidoEncRegionalPessoa.Equals(request.Text) && x.CondicaoPessoa.Equals("ALUNO"))
                .Select(x => x.ComumPessoa).Distinct();
            return comuns.ToList();
        }

    }
}

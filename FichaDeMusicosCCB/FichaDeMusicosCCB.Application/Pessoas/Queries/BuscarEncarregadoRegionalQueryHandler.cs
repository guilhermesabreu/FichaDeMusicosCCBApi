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
            var pessoaLogada = PessoaLogada(request).Result;
                return _context.Pessoas.AsNoTracking().Include(x => x.User)
                    .Where(x => x.NomePessoa.StartsWith(request.Input)
                    && x.User.Role.Equals("REGIONAL")
                    && x.RegiaoPessoa.Equals(pessoaLogada.RegiaoPessoa)
                    && x.RegionalPessoa.Equals(pessoaLogada.RegionalPessoa)).Select(x => x.NomePessoa).ToList();

            return new List<string>();
        }

        public async Task<Pessoa> PessoaLogada(BuscarEncarregadoRegionalQuery query)
        {
            return _context.Pessoas.AsNoTracking().Include(x => x.User)
                .Where(x => x.User.UserName.Equals(query.ApelidoPessoaLogada)).FirstOrDefault();
        }

    }
}

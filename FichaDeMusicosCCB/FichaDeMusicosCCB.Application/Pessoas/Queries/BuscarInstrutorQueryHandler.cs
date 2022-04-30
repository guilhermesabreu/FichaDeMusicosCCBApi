using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarInstrutorQueryHandler : IRequestHandler<BuscarInstrutorQuery, List<string>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public BuscarInstrutorQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<string>> Handle(BuscarInstrutorQuery request, CancellationToken cancellationToken)
        {
            var pessoaLogada = PessoaLogada(request).Result;
            if (pessoaLogada.CondicaoPessoa.ToUpper().Equals("INSTRUTOR") || pessoaLogada.CondicaoPessoa.ToUpper().Equals("ENCARREGADO"))
                return _context.Pessoas.AsNoTracking().Include(x => x.User)
                    .Where(x => x.NomePessoa.StartsWith(request.Input)
                    && x.User.Role.Equals("INSTRUTOR")
                    && x.ComumPessoa.Equals(pessoaLogada.ComumPessoa)
                    && x.RegiaoPessoa.Equals(pessoaLogada.RegiaoPessoa)
                    && x.RegionalPessoa.Equals(pessoaLogada.RegionalPessoa)).Select(x => x.NomePessoa).ToList();

            if (pessoaLogada.CondicaoPessoa.ToUpper().Equals("REGIONAL"))
                return _context.Pessoas.AsNoTracking().Include(x => x.User)
                    .Where(x => x.NomePessoa.StartsWith(request.Input)
                    && x.User.Role.Equals("INSTRUTOR")
                    && x.RegiaoPessoa.Equals(pessoaLogada.RegiaoPessoa)
                    && x.RegionalPessoa.Equals(pessoaLogada.RegionalPessoa)).Select(x => x.NomePessoa).ToList();

            return new List<string>();
        }

        public async Task<Pessoa> PessoaLogada(BuscarInstrutorQuery query)
        {
            return _context.Pessoas.AsNoTracking().Include(x => x.User)
                .Where(x => x.User.UserName.Equals(query.ApelidoPessoaLogada)).FirstOrDefault();
        }

    }
}

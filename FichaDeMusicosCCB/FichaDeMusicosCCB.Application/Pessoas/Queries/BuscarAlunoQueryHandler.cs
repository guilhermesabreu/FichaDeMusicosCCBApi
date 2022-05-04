using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class BuscarAlunoQueryHandler : IRequestHandler<BuscarAlunoQuery, List<string>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public BuscarAlunoQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<string>> Handle(BuscarAlunoQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ApelidoPessoaLogada))
                return _context.Pessoas.AsNoTracking().Include(x => x.User)
                    .Where(x => x.NomePessoa.StartsWith(request.Input)
                    && x.User.Role.Equals("ALUNO")).Select(x => x.NomePessoa).ToList();

            var pessoaLogada = PessoaLogada(request).Result;
            if(pessoaLogada.CondicaoPessoa.ToUpper().Equals("INSTRUTOR") || pessoaLogada.CondicaoPessoa.ToUpper().Equals("ENCARREGADO"))
                return _context.Pessoas.AsNoTracking().Include(x => x.User)
                    .Where(x => x.NomePessoa.StartsWith(request.Input) 
                    && x.User.Role.Equals("ALUNO")
                    && x.ComumPessoa.Equals(pessoaLogada.ComumPessoa) 
                    && x.RegiaoPessoa.Equals(pessoaLogada.RegiaoPessoa)
                    && x.RegionalPessoa.Equals(pessoaLogada.RegionalPessoa)).Select(x => x.NomePessoa).ToList();

            if (pessoaLogada.CondicaoPessoa.ToUpper().Equals("REGIONAL"))
                return _context.Pessoas.AsNoTracking().Include(x => x.User)
                    .Where(x => x.NomePessoa.StartsWith(request.Input)
                    && x.User.Role.Equals("ALUNO")
                    && x.RegiaoPessoa.Equals(pessoaLogada.RegiaoPessoa)
                    && x.RegionalPessoa.Equals(pessoaLogada.RegionalPessoa)).Select(x => x.NomePessoa).ToList();

            return new List<string>();
        }

        public async Task<Pessoa> PessoaLogada(BuscarAlunoQuery query)
        {
            return _context.Pessoas.AsNoTracking().Include(x => x.User)
                .Where(x => x.User.UserName.Equals(query.ApelidoPessoaLogada)).FirstOrDefault();
        } 

    }
}

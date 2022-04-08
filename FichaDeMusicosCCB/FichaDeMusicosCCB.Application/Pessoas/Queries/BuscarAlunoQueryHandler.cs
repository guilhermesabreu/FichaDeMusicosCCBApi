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
            var pessoas = _context.Pessoas.AsQueryable().Include(x => x.User);
            var encarregados = pessoas.Where(x => x.NomePessoa.StartsWith(request.Text) && x.User.Role.Equals("ALUNO")).Select(x => x.NomePessoa);
            return encarregados.ToList();
        }

    }
}

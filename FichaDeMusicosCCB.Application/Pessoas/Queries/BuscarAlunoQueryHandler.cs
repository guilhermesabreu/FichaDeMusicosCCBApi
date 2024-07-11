using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Queries
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
            try
            {
                if (request.Input.Length < 3)
                    return new List<string>();

                var pessoas = _context.Pessoas.AsNoTracking().Include(x => x.User)
                    .Where(x => x.NomePessoa.StartsWith(request.Input)
                    && x.User.Role.Equals("ALUNO")).Select(x => x.NomePessoa).Take(5).ToList();
                if (pessoas.Count == 0)
                    throw new ArgumentException("Aluno não encontrado");

                return pessoas;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(Utils.MensagemErro500Padrao);
            }

        }

        public async Task<Pessoa> PessoaLogada(BuscarAlunoQuery query)
        {
            return _context.Pessoas.AsNoTracking().Include(x => x.User)
                .Where(x => x.User.UserName.Equals(query.ApelidoPessoaLogada)).FirstOrDefault();
        }

    }
}

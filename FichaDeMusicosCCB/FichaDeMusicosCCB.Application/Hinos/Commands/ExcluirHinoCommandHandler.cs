using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Hinos.Commands
{
    public class ExcluirHinoCommandHandler : IRequestHandler<ExcluirHinoCommand, bool>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public ExcluirHinoCommandHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(ExcluirHinoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hinoEncontrado = await HinoEncontrado(request.IdHino);
                var exclusao = await ExcluirHino(hinoEncontrado);
                return exclusao;
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

        public async Task<bool> ExcluirHino(Hino hino)
        {
            _context.Remove(hino);
            if (await _context.SaveChangesAsync() == 1)
                return true;

            return false;
        }

        public async Task<Hino> HinoEncontrado(long idHino)
        {
            var hino = await _context.Hinos.AsNoTracking().Include(x => x.Pessoa).Where(x => x.IdHino == idHino).FirstOrDefaultAsync();
            if (hino == null)
                throw new ArgumentException("Hino não encontrado");
            
            var pessoaAluna = await _context.Pessoas.AsNoTracking().Where(x => x.IdPessoa == hino.IdPessoa && x.CondicaoPessoa.Equals("aluno")).FirstOrDefaultAsync();
            if (pessoaAluna == null)
                throw new ArgumentException("A pessoa atrelada a este hino não é um aluno.");

            return hino;
        }

    }
}

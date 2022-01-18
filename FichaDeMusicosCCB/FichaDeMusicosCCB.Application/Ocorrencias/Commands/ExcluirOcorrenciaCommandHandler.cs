using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Ocorrencias.Commands
{
    public class ExcluirOcorrenciaCommandHandler : IRequestHandler<ExcluirOcorrenciaCommand, bool>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public ExcluirOcorrenciaCommandHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(ExcluirOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ocorrenciaEncontrada = await OcorrenciaEncontrada(request.IdOcorrencia);
                var exclusao = await ExcluirOcorrencia(ocorrenciaEncontrada);
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

        public async Task<bool> ExcluirOcorrencia(Ocorrencia ocorrencia)
        {
            _context.Remove(ocorrencia);
            if (await _context.SaveChangesAsync() == 1)
                return true;

            return false;
        }

        public async Task<Ocorrencia> OcorrenciaEncontrada(long idOcorrencia)
        {
            var ocorrencia = await _context.Ocorrencias.AsNoTracking().Include(x => x.Pessoa).Where(x => x.IdOcorrencia == idOcorrencia).FirstOrDefaultAsync();
            if (ocorrencia == null)
                throw new ArgumentException("Ocorrencia não encontrada");
            
            var pessoaAluna = await _context.Pessoas.AsNoTracking().Where(x => x.IdPessoa == ocorrencia.IdPessoa && x.CondicaoPessoa.Equals("aluno")).FirstOrDefaultAsync();
            if (pessoaAluna == null)
                throw new ArgumentException("A pessoa atrelada a esta ocorrência não é um aluno.");


            return ocorrencia;
        }

    }
}

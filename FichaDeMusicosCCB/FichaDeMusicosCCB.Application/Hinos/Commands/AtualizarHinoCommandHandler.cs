using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Hinos.Commands
{
    public class AtualizarHinoCommandHandler : IRequestHandler<AtualizarHinoCommand, HinoViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public AtualizarHinoCommandHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<HinoViewModel> Handle(AtualizarHinoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region MapearParametro
                TypeAdapterConfig<AtualizarHinoCommand, Hino>.NewConfig()
                .Map(dest => dest.NumeroHino, src => src.Numero)
                .Map(dest => dest.VozHino, src => src.Voz)
                .Map(dest => dest.DataHino, src => src.Data);
                #endregion
                var hinoAtual = request.Adapt<Hino>();

                var hinoAntigo = await HinoEncontrado(hinoAtual.IdHino, hinoAtual.IdPessoa);

                var hinoResponse = await HinoAtualizado(hinoAntigo, hinoAtual);
                return hinoResponse.Adapt<HinoViewModel>();
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

        public async Task<Hino> HinoAtualizado(Hino hinoAntigo, Hino hinoAtual)
        {
            var hinoAtualizado = hinoAntigo;
            hinoAtualizado.NumeroHino = hinoAtual.NumeroHino;
            hinoAtualizado.VozHino = hinoAtual.VozHino;
            hinoAtualizado.DataHino = hinoAtual.DataHino;

            _context.Hinos.Update(hinoAtualizado);
            if (_context.SaveChanges().Equals(0))
                throw new ArgumentException("Não foi possível atualizar este Hino, verifique os dados inseridos");

            return hinoAtual;
        }

        public async Task<Hino> HinoEncontrado(long idHino, long idPessoa)
        {
            var hinos = await _context.Hinos.AsNoTracking().Include(x => x.Pessoa).AsNoTracking().Include(x => x.Pessoa).Where(x => x.IdHino == idHino).FirstOrDefaultAsync();
            if (hinos == null)
                throw new ArgumentException("Hino não encontrado");

            var pessoaAluna = await _context.Pessoas.AsNoTracking().Where(x => x.IdPessoa == hinos.IdPessoa && x.CondicaoPessoa.Equals("aluno")).FirstOrDefaultAsync();
            if (pessoaAluna == null)
                throw new ArgumentException("A pessoa atrelada a este hino não é um aluno.");

            if (pessoaAluna.IdPessoa != idPessoa)
                throw new ArgumentException("Este hino já está atrelado a outro aluno");


            if (hinos.Pessoa == null)
                throw new ArgumentException("Esta ocorrência não está atrelada a nenhuma pessoa");

            return hinos;
        }
    }
}

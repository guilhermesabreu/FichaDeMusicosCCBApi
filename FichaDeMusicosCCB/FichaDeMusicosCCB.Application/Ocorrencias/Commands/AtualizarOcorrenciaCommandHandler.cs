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
    public class AtualizarOcorrenciaCommandHandler : IRequestHandler<AtualizarOcorrenciaCommand, OcorrenciaViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public AtualizarOcorrenciaCommandHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<OcorrenciaViewModel> Handle(AtualizarOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region MapearParametro
                TypeAdapterConfig<AtualizarOcorrenciaCommand, Ocorrencia>.NewConfig()
                    .Map(dest => dest.DataOcorrencia, src => DateTimeOffset.Parse(src.DataOcorrencia).UtcDateTime)
                    .Map(dest => dest.NumeroLicaoOcorrencia, src => src.NumeroLicao)
                    .Map(dest => dest.MetodoOcorrencia, src => src.NomeMetodo)
                    .Map(dest => dest.ObservacaoOcorrencia, src => src.ObservacaoInstrutor);
                #endregion
                var ocorrenciaAtual = request.Adapt<Ocorrencia>();

                var ocorrenciaAntiga = await OcorrenciaEncontrada(ocorrenciaAtual.IdOcorrencia, ocorrenciaAtual.IdPessoa);
                #region Mapear Response
                TypeAdapterConfig<Ocorrencia, OcorrenciaViewModel>.NewConfig()
                    .Map(dest => dest.NumeroLicao, src => src.NumeroLicaoOcorrencia)
                    .Map(dest => dest.NomeMetodo, src => src.MetodoOcorrencia)
                    .Map(dest => dest.ObservacaoInstrutor, src => src.ObservacaoOcorrencia);
                #endregion

                var ocorrenciaResponse = await OcorrenciaAtualizada(ocorrenciaAntiga, ocorrenciaAtual);
                return ocorrenciaResponse.Adapt<OcorrenciaViewModel>();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Ocorrencia> OcorrenciaAtualizada(Ocorrencia ocorrenciaAntiga, Ocorrencia ocorrenciaAtual)
        {
            var ocorrenciaAtualizada = ocorrenciaAntiga;
            ocorrenciaAtualizada.MetodoOcorrencia = ocorrenciaAtual.MetodoOcorrencia;
            ocorrenciaAtualizada.NumeroLicaoOcorrencia = ocorrenciaAtual.NumeroLicaoOcorrencia;
            ocorrenciaAtualizada.DataOcorrencia = ocorrenciaAtual.DataOcorrencia;
            ocorrenciaAtualizada.ObservacaoOcorrencia = ocorrenciaAtual.ObservacaoOcorrencia;

            _context.Ocorrencias.Update(ocorrenciaAtualizada);
            if (_context.SaveChanges().Equals(0))
                throw new ArgumentException("Não foi possível atualizar esta ocorrência, verifique os dados inseridos");

            return ocorrenciaAtual;
        }

        public async Task<Ocorrencia> OcorrenciaEncontrada(long idOcorrencia, long idPessoa)
        {
            var ocorrencias = await _context.Ocorrencias.AsNoTracking().Include(x => x.Pessoa).AsNoTracking().Include(x => x.Pessoa).Where(x => x.IdOcorrencia == idOcorrencia).FirstOrDefaultAsync();
            if (ocorrencias == null)
                throw new ArgumentException("Ocorrência não encontrada");

            var pessoaAluna = await _context.Pessoas.AsNoTracking().Where(x => x.IdPessoa == ocorrencias.IdPessoa && x.CondicaoPessoa.Equals("aluno")).FirstOrDefaultAsync();
            if (pessoaAluna == null)
                throw new ArgumentException("A pessoa atrelada a esta ocorrência não é um aluno.");

            if(pessoaAluna.IdPessoa != idPessoa)
                throw new ArgumentException("Esta ocorrência já está atrelada a outro aluno");


            if(ocorrencias.Pessoa == null)
                throw new ArgumentException("Esta ocorrência não está atrelada a nenhuma pessoa");

            return ocorrencias;
        }
    }
}

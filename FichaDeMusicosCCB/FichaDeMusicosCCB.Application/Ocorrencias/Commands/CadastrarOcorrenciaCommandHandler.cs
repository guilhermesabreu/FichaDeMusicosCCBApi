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
    public class CadastrarOcorrenciaCommandHandler : IRequestHandler<CadastrarOcorrenciaCommand, OcorrenciaViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        private readonly UserManager<User> _userManager;
        public CadastrarOcorrenciaCommandHandler(FichaDeMusicosCCBContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<OcorrenciaViewModel> Handle(CadastrarOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region MapearParametro
                TypeAdapterConfig<CadastrarOcorrenciaCommand, Ocorrencia>.NewConfig()
                    .Map(dest => dest.DataOcorrencia, src => src.DataOcorrencia)
                    .Map(dest => dest.NumeroLicaoOcorrencia, src => src.NumeroLicao)
                    .Map(dest => dest.MetodoOcorrencia, src => src.NomeMetodo)
                    .Map(dest => dest.ObservacaoOcorrencia, src => src.ObservacaoInstrutor);
                #endregion
                var ocorrenciaEntity = request.Adapt<Ocorrencia>();

                ocorrenciaEntity = await VerificaExistenciaOcorrencia(ocorrenciaEntity);
                #region Mapear Response
                TypeAdapterConfig<Ocorrencia, OcorrenciaViewModel>.NewConfig()
                    .Map(dest => dest.NumeroLicao, src => src.NumeroLicaoOcorrencia)
                    .Map(dest => dest.NomeMetodo, src => src.MetodoOcorrencia)
                    .Map(dest => dest.ObservacaoInstrutor, src => src.ObservacaoOcorrencia);
                #endregion

                var ocorrenciaResponse = await OcorrenciaCriada(ocorrenciaEntity);
                return ocorrenciaResponse.Adapt<OcorrenciaViewModel>();
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

        public async Task<Ocorrencia> OcorrenciaCriada(Ocorrencia ocorrencia)
        {
            _context.Ocorrencias.Add(ocorrencia);
            if (_context.SaveChanges().Equals(0))
                throw new ArgumentException("Não foi possível criar esta ocorrência");

            return ocorrencia;

        }

        public async Task<Ocorrencia> VerificaExistenciaOcorrencia(Ocorrencia ocorrencia)
        {
            var ocorrenciaEntity = await _context.Ocorrencias.AsNoTracking().Where(x => x.MetodoOcorrencia == ocorrencia.MetodoOcorrencia
                                                    && x.NumeroLicaoOcorrencia == ocorrencia.NumeroLicaoOcorrencia
                                                    && x.IdPessoa == ocorrencia.IdPessoa).ToListAsync();

            var pessoaAluna = await _context.Pessoas.Where(x => x.IdPessoa == ocorrencia.IdPessoa && x.CondicaoPessoa.Equals("aluno")).FirstOrDefaultAsync();
            if(pessoaAluna == null)
                throw new ArgumentException("Esta pessoa não é um aluno.");

            if (ocorrenciaEntity.Count > 0)
                throw new ArgumentException("Esta ocorrência já foi cadastrada.");

            ocorrencia.Pessoa = pessoaAluna;
            return ocorrencia; 
        }
    }
}

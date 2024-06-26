﻿using FichaDeMusicosCCB.Domain.Commoms;
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

        public async Task<Ocorrencia> VerificaExistenciaOcorrencia(Ocorrencia ocorrenciaAtual)
        {

            var pessoaAluna = await _context.Pessoas.Where(x => x.IdPessoa == ocorrenciaAtual.IdPessoa && x.CondicaoPessoa.Equals("aluno")).FirstOrDefaultAsync();
            if(pessoaAluna == null)
                throw new ArgumentException("Esta pessoa não é um aluno.");

            if (ocorrenciaAtual.DataOcorrencia.HasValue && ocorrenciaAtual.DataOcorrencia.Value.Date > DateTime.Now.Date)
                throw new ArgumentException("Escolha uma data anterior a esta.");

            var ocorrenciaEntity = await _context.Ocorrencias.AsNoTracking().Where(x => x.DataOcorrencia!.Value.Date == ocorrenciaAtual.DataOcorrencia!.Value.Date
                                                    && x.IdPessoa == ocorrenciaAtual.IdPessoa).ToListAsync();
            if (ocorrenciaEntity.Count > 0)
                throw new ArgumentException($"Já foi cadastrado uma ocorrência nesse mesmo dia: {ocorrenciaAtual.DataOcorrencia.Value.Date}");

            ocorrenciaAtual.Pessoa = pessoaAluna;
            return ocorrenciaAtual; 
        }
    }
}

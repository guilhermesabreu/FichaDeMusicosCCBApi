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
    public class CadastrarHinoCommandHandler : IRequestHandler<CadastrarHinoCommand, HinoViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public CadastrarHinoCommandHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<HinoViewModel> Handle(CadastrarHinoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region MapearParametro
                TypeAdapterConfig<CadastrarHinoCommand, Hino>.NewConfig()
                    .Map(dest => dest.NumeroHino, src => src.Numero)
                    .Map(dest => dest.DataHino, src => src.Data)
                    .Map(dest => dest.VozHino, src => src.Voz);
                #endregion
                var hinoEntity = request.Adapt<Hino>();

                hinoEntity = await VerificaExistenciaHino(hinoEntity);
                
                var hinoResponse = await HinoCriado(hinoEntity);

                #region Mapeamento Response
                TypeAdapterConfig<Hino, HinoViewModel>.NewConfig()
                        .Map(dest => dest.DataHino, src => Utils.DataString(src.DataHino));
                #endregion
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

        public async Task<Hino> HinoCriado(Hino hino)
        {
            _context.Hinos.Add(hino);
            if (_context.SaveChanges().Equals(0))
                throw new ArgumentException("Não foi possível criar este hino.");

            return hino;

        }

        public async Task<Hino> VerificaExistenciaHino(Hino hino)
        {
            var hinoEntity = await _context.Hinos.AsNoTracking().Where(x => x.NumeroHino == hino.NumeroHino
                                                    && x.VozHino == hino.VozHino
                                                    && x.IdPessoa == hino.IdPessoa).ToListAsync();

            var pessoaAluna = await _context.Pessoas.Where(x => x.IdPessoa == hino.IdPessoa && x.CondicaoPessoa.Equals("aluno")).FirstOrDefaultAsync();
            if(pessoaAluna == null)
                throw new ArgumentException("Esta pessoa não é um aluno.");

            if (hinoEntity.Count > 0)
                throw new ArgumentException("Este hino já foi cadastrado.");


            hino.Pessoa = pessoaAluna;
            return hino; 
        }
    }
}

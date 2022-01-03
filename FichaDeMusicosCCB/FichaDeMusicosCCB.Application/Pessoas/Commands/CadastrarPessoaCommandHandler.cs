using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FichaDeMusicosCCB.Application.Pessoas.Commands
{
    public class CadastrarPessoaCommandHandler : IRequestHandler<CadastrarPessoaCommand, PessoaViewModel>
    {
        private readonly FichaDeMusicosCCBContext _context;
        private readonly UserManager<User> _userManager;
        public CadastrarPessoaCommandHandler(FichaDeMusicosCCBContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<PessoaViewModel> Handle(CadastrarPessoaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region MapearParametro
                TypeAdapterConfig<CadastrarPessoaCommand, FichaDeMusicosCCB.Domain.Entities.Pessoa>.NewConfig()
                    .Map(dest => dest.User.UserName, src => src.UserName)
                    .Map(dest => dest.User.Password, src => src.Password)
                    .Map(dest => dest.NomePessoa, src => src.Nome)
                    .Map(dest => dest.ApelidoInstrutorPessoa, src => src.ApelidoInstrutor)
                    .Map(dest => dest.ApelidoEncarregadoPessoa, src => src.ApelidoEncarregado)
                    .Map(dest => dest.ApelidoEncRegionalPessoa, src => src.ApelidoEncRegional)
                    .Map(dest => dest.RegiaoPessoa, src => src.Regiao)
                    .Map(dest => dest.RegionalPessoa, src => src.Regional)
                    .Map(dest => dest.CelularPessoa, src => src.Celular)
                    .Map(dest => dest.EmailPessoa, src => src.Email)
                    .Map(dest => dest.DataNascimentoPessoa, src => src.DataNascimento)
                    .Map(dest => dest.DataInicioPessoa, src => src.DataInicio)
                    .Map(dest => dest.ComumPessoa, src => src.Comum)
                    .Map(dest => dest.InstrumentoPessoa, src => src.Instrumento)
                    .Map(dest => dest.CondicaoPessoa, src => src.Condicao);
                #endregion
                var pessoaEntity = request.Adapt<FichaDeMusicosCCB.Domain.Entities.Pessoa>();

                await CriarRoles();
                await VerificaExistenciaPessoa(pessoaEntity);
                #region Mapear Response
                TypeAdapterConfig<FichaDeMusicosCCB.Domain.Entities.Pessoa, PessoaViewModel>.NewConfig()
                    .Map(dest => dest.Nome, src => src.NomePessoa)
                    .Map(dest => dest.ApelidoInstrutor, src => src.ApelidoInstrutorPessoa)
                    .Map(dest => dest.ApelidoEncarregado, src => src.ApelidoEncarregadoPessoa)
                    .Map(dest => dest.ApelidoEncRegional, src => src.ApelidoEncRegionalPessoa)
                    .Map(dest => dest.Regiao, src => src.RegiaoPessoa)
                    .Map(dest => dest.Regional, src => src.RegionalPessoa)
                    .Map(dest => dest.Celular, src => src.CelularPessoa)
                    .Map(dest => dest.Email, src => src.EmailPessoa)
                    .Map(dest => dest.DataNascimento, src => src.DataNascimentoPessoa)
                    .Map(dest => dest.DataInicio, src => src.DataInicioPessoa)
                    .Map(dest => dest.Comum, src => src.ComumPessoa)
                    .Map(dest => dest.Instrumento, src => src.InstrumentoPessoa)
                    .Map(dest => dest.Condicao, src => src.CondicaoPessoa);
                #endregion

                var pessoaResponse = await PessoaCriada(pessoaEntity);
                return pessoaResponse.Adapt<PessoaViewModel>();
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

        public async Task<FichaDeMusicosCCB.Domain.Entities.Pessoa> PessoaCriada(FichaDeMusicosCCB.Domain.Entities.Pessoa pessoa)
        {
            pessoa.User.Role = pessoa.CondicaoPessoa.ToUpper();
            var result = await _userManager.CreateAsync(pessoa.User, pessoa.User.Password);
            var resultRole = await _userManager.AddToRoleAsync(pessoa.User, pessoa.User.Role);
            if (!resultRole.Succeeded)
                throw new ArgumentException("Não foi possível criar a credencial desta pessoa");

            _context.Pessoas.Add(pessoa);
            if (_context.SaveChanges().Equals(0) || !result.Succeeded)
                throw new ArgumentException("Não foi possível criar esta pessoa, verifique os dados inseridos");

            return pessoa;

        }


        public async Task VerificaExistenciaPessoa(FichaDeMusicosCCB.Domain.Entities.Pessoa pessoa)
        {
            var query = await _context.Users.Where(x => x.UserName == pessoa.User.UserName).ToListAsync();

            if (query.Count > 0)
                throw new ArgumentException("Esta pessoa já está cadastrada");

        }

        public async Task CriarRoles()
        {
            var roles = await _context.Roles.Select(x => x.Name).ToListAsync();
            lock (_context)
            {
                var contemRoles = roles.Any(x => x == "ENCARREGADO" || x == "REGIONAL" || x == "INSTRUTOR");

                if (!contemRoles)
                {
                    Role roleEncarregado = new Role();
                    roleEncarregado.Name = "ENCARREGADO";
                    roleEncarregado.NormalizedName = "ENCARREGADO";
                    Role roleRegional = new Role();
                    roleRegional.Name = "REGIONAL";
                    roleRegional.NormalizedName = "REGIONAL";
                    Role roleInstrutor = new Role();
                    roleInstrutor.Name = "INSTRUTOR";
                    roleInstrutor.NormalizedName = "INSTRUTOR";
                    _context.Roles.Add(roleEncarregado);
                    _context.SaveChangesAsync();
                    _context.Roles.Add(roleRegional);
                    _context.SaveChangesAsync();
                    _context.Roles.Add(roleInstrutor);
                    _context.SaveChangesAsync();

                }
            }

        }
    }
}

using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Commands
{
    public class ExcluirPessoaNaFichaCommandHandler : IRequestHandler<ExcluirPessoaNaFichaCommand, bool>
    {
        private readonly FichaDeMusicosCCBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public ExcluirPessoaNaFichaCommandHandler(FichaDeMusicosCCBContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<bool> Handle(ExcluirPessoaNaFichaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var pessoaEncontrada = await UsuarioEncontrado(request.IdPessoa);
                var exclusao = await ExcluirPessoa(pessoaEncontrada, request.ApelidoDonoDaFicha);
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

        public async Task<bool> ExcluirPessoa(User user, string donoDaFicha)
        {
            var pessoaFicha = _context.Pessoas.AsNoTracking().Include(x => x.User).Where(x => x.User.UserName.Equals(donoDaFicha)).FirstOrDefault();
            if (pessoaFicha == null)
                throw new ArgumentException("Você não foi encontrado na base de dados");

            user.Pessoa.ApelidoInstrutorPessoa = string.IsNullOrEmpty(user.Pessoa.ApelidoInstrutorPessoa) ? "" : user.Pessoa.ApelidoInstrutorPessoa;
            user.Pessoa.ApelidoEncarregadoPessoa = string.IsNullOrEmpty(user.Pessoa.ApelidoEncarregadoPessoa) ? "" : user.Pessoa.ApelidoEncarregadoPessoa;
            user.Pessoa.ApelidoEncRegionalPessoa = string.IsNullOrEmpty(user.Pessoa.ApelidoEncRegionalPessoa) ? "" : user.Pessoa.ApelidoEncRegionalPessoa;
            if (pessoaFicha.CondicaoPessoa.ToUpper().Equals("INSTRUTOR"))
            {
                user.Pessoa.ApelidoInstrutorPessoa = user.Pessoa.ApelidoInstrutorPessoa.Contains(pessoaFicha.User.UserName)
                                                ? user.Pessoa.ApelidoInstrutorPessoa.Replace(";" + pessoaFicha.User.UserName, "")
                                                : user.Pessoa.ApelidoInstrutorPessoa;

                _context.Pessoas.Update(user.Pessoa);
                _context.SaveChanges();
                return true;

            }
            else
            {
                _context.Remove(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<User> UsuarioEncontrado(long idPessoa)
        {
            var query = await _context.Users.AsNoTracking().Include(x => x.Pessoa).Where(x => x.Id == idPessoa).FirstOrDefaultAsync();
            if (query == null)
                throw new ArgumentException("Usuário não encontrado");

            return query;
        }

    }
}

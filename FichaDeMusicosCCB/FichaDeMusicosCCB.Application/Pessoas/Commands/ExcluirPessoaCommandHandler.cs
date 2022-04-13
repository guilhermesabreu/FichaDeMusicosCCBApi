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
    public class ExcluirPessoaCommandHandler : IRequestHandler<ExcluirPessoaCommand, bool>
    {
        private readonly FichaDeMusicosCCBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public ExcluirPessoaCommandHandler(FichaDeMusicosCCBContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<bool> Handle(ExcluirPessoaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var pessoaEncontrada = await UsuarioEncontrado(request.IdPessoa);
                var exclusao = await ExcluirPessoa(pessoaEncontrada);
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

        public async Task<bool> ExcluirPessoa(User user)
        {
            _context.Remove(user);
            _context.SaveChanges();
            return true;
        }


        public async Task<User> UsuarioEncontrado(long idPessoa)
        {
            var query = await _context.Users.AsNoTracking().Where(x => x.Id == idPessoa).FirstOrDefaultAsync();
            if (query == null)
                throw new ArgumentException("Usuário não encontrado");

            return query;
        }

    }
}

using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Alunos.Commands
{
    public class ExcluirAlunoCommandHandler : IRequestHandler<ExcluirAlunoCommand, bool>
    {
        private readonly FichaDeMusicosCCBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public ExcluirAlunoCommandHandler(FichaDeMusicosCCBContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<bool> Handle(ExcluirAlunoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var alunoEncontrada = await UsuarioEncontrado(request.IdAluno);
                var exclusao = await ExcluirAluno(alunoEncontrada);
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

        public async Task<bool> ExcluirAluno(User user)
        {
            _context.Remove(user);
            if (await _context.SaveChangesAsync() == 1)
                return true;

            return false;
        }

        public async Task<User> UsuarioEncontrado(long idPessoa)
        {
            var pessoa = await _context.Pessoas.AsNoTracking().Include(x => x.User).Where(x => x.IdPessoa == idPessoa).FirstOrDefaultAsync();
            if (pessoa == null)
                throw new ArgumentException("Pessoa não encontrada");

            if(!pessoa.CondicaoPessoa.ToUpper().Equals("ALUNO"))
                throw new ArgumentException("Esta pessoa não é um aluno");

            var usuario = await _context.Users.AsNoTracking().Where(x => x.UserName == pessoa.User.UserName).FirstOrDefaultAsync();
            if (usuario == null)
                throw new ArgumentException("Usuário não encontrado");

            return usuario;
        }

    }
}

using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FichaDeMusicosCCB.Application.Pessoas.Commands
{
    public class LogarPessoaCommandHandler : IRequestHandler<LogarPessoaCommand, string>
    {
        private readonly FichaDeMusicosCCBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        public LogarPessoaCommandHandler(FichaDeMusicosCCBContext context, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<string> Handle(LogarPessoaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var usuario = await UsuarioEncontrado(request.UserName, request.Password);
                await CheckarLogin(usuario, request.Password);
                var token = await RecuperarToken(usuario);
                return token;
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

        public async Task<User> UsuarioEncontrado(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new ArgumentException("Usuario não encontrado");

            return user;
        }

        public async Task CheckarLogin(User user, string senha)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, senha, false);
            if (!result.Succeeded)
                throw new ArgumentException("Usuario ou senha incorretas");

        }

        public async Task<string> RecuperarToken(User user)
        {
            var role = await _userManager.GetRolesAsync(user);
            IdentityOptions _options = new IdentityOptions();

            var key = new SymmetricSecurityKey(Encoding.ASCII
            .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                            new Claim("UserId", user.Id.ToString()),
                            new Claim(_options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault())
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token não registrado, não foi possível realizar o Login");
            
            return token;
        }

    }
}

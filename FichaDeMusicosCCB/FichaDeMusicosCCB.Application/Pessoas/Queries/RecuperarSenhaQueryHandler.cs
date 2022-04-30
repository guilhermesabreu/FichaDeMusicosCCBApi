using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class RecuperarSenhaQueryHandler : IRequestHandler<RecuperarSenhaQuery, bool>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public RecuperarSenhaQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(RecuperarSenhaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pessoaLogada = await PessoaLogada(request);
                if (EnvioDeEmail(pessoaLogada))
                    return true;

                return false;
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

        public async Task<Pessoa> PessoaLogada(RecuperarSenhaQuery query)
        {
            var pessoaLogada = _context.Pessoas.AsNoTracking().Include(x => x.User)
                .Where(x => x.User.UserName.Equals(query.Email)).FirstOrDefault();

            if (string.IsNullOrEmpty(pessoaLogada.EmailPessoa))
                throw new ArgumentException("E-mail não cadastrado !");

            return pessoaLogada;

        }

        public static bool EnvioDeEmail(Pessoa pessoa)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential("guilhermeabreucc@gmail.com", "cipofaykpagjrqlf");
                using (MailMessage message = new MailMessage())
                {
                    try
                    {
                        MailAddress fromAddress = new MailAddress("guilhermeabreucc@gmail.com");
                        smtpClient.Host = "smtp.gmail.com";
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = basicCredential;
                        message.From = fromAddress;
                        message.Subject = "Ficha de Músicos CCB";
                        message.IsBodyHtml = true;
                        message.Body = BodyPadrao(pessoa);
                        message.To.Add(pessoa.EmailPessoa);
                        smtpClient.Send(message);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    return false;
                }
            }
        }

        public static string BodyPadrao(Pessoa pessoa)
        {
            return "<h3>Prezado Irmão</h3>" + pessoa.NomePessoa +
                     "<p>Seu Login é: <b>" + pessoa.User.UserName + "</b></p> " +
                     "<p>e sua Senha é: <b>" + pessoa.User.Password + "</b></p>. " +
                     "<p>Guarde com cuidado!</p> " +
                     "<p>Entre novamente em sua ficha e fique por dentro de tudo <b>https://agendaonlinewebapp.herokuapp.com/</b>" +
                     "<p><i>Att. Ficha de Músicos CCB</i></p>";
        }

    }
}

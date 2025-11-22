using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Projeto.Services
{
    public class EmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _from;

        public EmailService(string smtpHost, int smtpPort, string smtpUser, string smtpPass, string from)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
            _from = from;
        }

        public void Enviar(string para, string assunto, string corpoHtml)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Equipe", _from));
            message.To.Add(MailboxAddress.Parse(para));
            message.Subject = assunto;

            var body = new BodyBuilder { HtmlBody = corpoHtml };
            message.Body = body.ToMessageBody();

            using var client = new SmtpClient();
            client.Connect(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_smtpUser, _smtpPass);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}

using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.MailService
{
    public sealed class MailService
    {
        private readonly MailServiceOptions _options;

        public MailService(IOptions<MailServiceOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_options.Host, _options.Port, _options.UseSsl);
                await client.AuthenticateAsync(_options.FromAddress, _options.FromPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}

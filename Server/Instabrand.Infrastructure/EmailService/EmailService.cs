﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.EmailService
{
    public sealed class EmailService : Domain.Registration.IConfirmationCodeSender
    {
        private readonly string _smtpServer;
        private readonly string _fromAddress;
        private readonly string _password;
        private readonly int _port;

        public EmailService(IOptions<EmailServiceOptions> options)
        {
            _smtpServer = options.Value.SmtpServer;
            _fromAddress = options.Value.FromAddress;
            _password = options.Value.Password;
            _port = options.Value.Port;
        }

        public async Task SendConfirmationCode(string email, string confirmationCode, CancellationToken cancellationToken)
        {
            string message = $@"Welcome to Boxis.io (www.boxis.io).
<br /><br />
Please confirm your email: <br />
<a href='https://boxis.io/registrations/confirm/?confirmationCode={confirmationCode}'>https://boxis.io/registrations/confirm/?confirmationCode={confirmationCode}</a>";

            await SendEmailAsync(email, "BOXIS - Please confirm your E-mail address", message, cancellationToken);
        }

        private async Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Boxis.io", _fromAddress));
            emailMessage.To.Add(new MailboxAddress("Dear customer", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _port, true, cancellationToken);
                await client.AuthenticateAsync(_fromAddress, _password, cancellationToken);
                await client.SendAsync(emailMessage, cancellationToken);

                await client.DisconnectAsync(true);
            }
        }
    }
}

namespace Instabrand.Infrastructure.EmailService
{
    public sealed class EmailServiceOptions
    {
        public string SmtpServer { get; set; }
        public string FromAddress { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}

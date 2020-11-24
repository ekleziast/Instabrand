namespace Instabrand.Infrastructure.MailService
{
    public class MailServiceOptions
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string FromPassword { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
    }
}

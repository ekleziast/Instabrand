namespace Instabrand.Infrastructure.UserRegistration
{
    public class ConfirmationCodeProviderOptions
    {
        public string SecretKey { get; set; }

        public int LifeTimeCodeInMinute { get; set; }
    }
}

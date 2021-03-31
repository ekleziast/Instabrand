namespace Instabrand.Infrastructure.ConfirmationCodeProvider
{
    public sealed class ConfirmationCodeProviderOptions
    {
        public string SecretKey { get; set; }
        public int LifeTimeCodeInMinute { get; set; }
    }
}

namespace Instabrand.Infrastructure.Authentication
{
    public sealed class JwtAuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int LifeTimeMinutes { get; set; }
    }
}

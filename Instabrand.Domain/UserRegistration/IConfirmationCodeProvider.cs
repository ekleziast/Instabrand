namespace Instabrand.Domain.UserRegistration
{
    public interface IConfirmationCodeProvider
    {
        string Generate(string email);
        bool Validate(in string token, out string email);
    }
}

namespace Instabrand.Domain.UserRegistration
{
    public interface IConfirmationCodeSender
    {
        void Send(string email, string confirmationCode);
    }
}

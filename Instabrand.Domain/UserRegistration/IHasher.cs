namespace Instabrand.Domain.UserRegistration
{
    public interface IHasher
    {
        string Hash(string password);
    }
}

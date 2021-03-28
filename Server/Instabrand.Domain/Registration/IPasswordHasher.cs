namespace Instabrand.Domain.Registration
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
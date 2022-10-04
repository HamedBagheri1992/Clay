namespace SSO.Application.Contracts.Infrastructure
{
    public interface IEncryptionService
    {
        string HashPassword(string pass);
    }
}

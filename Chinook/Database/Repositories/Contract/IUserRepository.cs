namespace Chinook.Database.Repositories.Contract
{
    public interface IUserRepository
    {
        Task<string> GetUserId();
    }
}

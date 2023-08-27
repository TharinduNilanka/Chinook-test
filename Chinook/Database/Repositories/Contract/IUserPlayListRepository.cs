using Chinook.ClientModels;

namespace Chinook.Database.Repositories.Contract
{
    public interface IUserPlayListRepository
    {
        Task<List<UserPlayList>> GetPlayListByUserIdAsync();
    }
}

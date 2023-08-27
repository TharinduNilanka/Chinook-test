using Chinook.ClientModels;

namespace Chinook.Database.Repositories.Contract
{
    public interface IPlayListRepository
    {
        Task<Playlist> GetPlayListByIdAsync(long playListId);
        Task AddTrackToPlayList(long trackId, string playListName, long? playListId = null);
        Task MarkTrackUnFavorite(long trackId);
    }
}

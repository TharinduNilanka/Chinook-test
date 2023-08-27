using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Database.Repositories.Contract
{
    public interface ITracksRepository
    {
        Task<List<PlaylistTrack>> GetTracksByArtistIdAsync(long ArtistId);
    }
}

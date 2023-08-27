using Chinook.ClientModels;
using Chinook.Database.Repositories.Contract;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Database.Repositories
{
    public class TracksRepository : ITracksRepository
    {
        private readonly ChinookContext _dbContext;
        private readonly IUserRepository _userRepository;
        public TracksRepository(ChinookContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }
        public async Task<List<PlaylistTrack>> GetTracksByArtistIdAsync(long ArtistId)
        {
            var currentUserId = await _userRepository.GetUserId();
            return await _dbContext.Tracks.Where(a => a.Album.ArtistId == ArtistId)
                 .Include(a => a.Album)
                 .Select(t => new PlaylistTrack()
                 {
                     AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                     TrackId = t.TrackId,
                     TrackName = t.Name,
                     IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == "Favorites")).Any()
                 })
                 .ToListAsync();
        }
    }
}

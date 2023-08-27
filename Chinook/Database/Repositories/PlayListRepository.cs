using Chinook.ClientModels;
using Chinook.Database.Repositories.Contract;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;
using PlaylistDto = Chinook.ClientModels.Playlist;
using Playlist = Chinook.Models.Playlist;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Database.Repositories
{
    public class PlayListRepository : IPlayListRepository
    {
        private readonly ChinookContext _dbContext;
        private readonly IUserRepository _userRepository;
        public PlayListRepository(ChinookContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }
        public async Task<PlaylistDto> GetPlayListByIdAsync(long playListId)
        {
            var currentUserId = await _userRepository.GetUserId();
            return await _dbContext.Playlists
                .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                .Where(p => p.PlaylistId == playListId)
                .Select(p => new PlaylistDto()
                {
                    Name = p.Name,
                    Tracks = p.Tracks.Select(t => new PlaylistTrack()
                    {
                        AlbumTitle = t.Album.Title,
                        ArtistName = t.Album.Artist.Name,
                        TrackId = t.TrackId,
                        TrackName = t.Name,
                        IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == "Favorites")).Any()
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddTrackToPlayList(long trackId, string playListName,long? playListId = null)
        {
            var currentUserId = await _userRepository.GetUserId();
            var playList = new Playlist();

            if(playListName == null && playListId == null)
            {
                throw new Exception();
            } 

            if(playListId != null)
            { 
                playList = await _dbContext.Playlists.Where(x=>x.PlaylistId == playListId).FirstOrDefaultAsync();
            }

            if(playListName != null) {
                playList = await CreatePlayList(playListName);
            }
            
            var track = await _dbContext.Tracks.FirstOrDefaultAsync(x => x.TrackId == trackId);

            if (track != null)
            {
                playList.Tracks.Add(track);

            }
            await _dbContext.SaveChangesAsync();
        }

        private async Task<Playlist> CreatePlayList(string PlayListName)
        {
            var currentUserId = await _userRepository.GetUserId();

            var playlist = await _dbContext.Playlists.Include(t => t.UserPlaylists).Include(x => x.Tracks).SingleOrDefaultAsync(p => p.Name == PlayListName && p.UserPlaylists.Any(up => up.UserId == currentUserId));
            var playListId = _dbContext.Playlists.Count();
            if (playlist == null)
            {
                playlist = new Playlist { Name = PlayListName, PlaylistId = playListId };
                await _dbContext.Playlists.AddAsync(playlist);
                await _dbContext.UserPlaylists.AddAsync(new UserPlaylist { UserId = currentUserId, PlaylistId = playlist.PlaylistId });
                await _dbContext.SaveChangesAsync();
            }

            return playlist;
        }

        public async Task MarkTrackUnFavorite(long trackId)
        {
            var currentUserId = await _userRepository.GetUserId();

            var favouritePlaylist = await _dbContext.Playlists.Include(t => t.UserPlaylists).Include(x => x.Tracks).SingleOrDefaultAsync(p => p.Name == "Favorites" && p.UserPlaylists.Any(up => up.UserId == currentUserId));

            var track = await _dbContext.Tracks.FirstOrDefaultAsync(x => x.TrackId == trackId);

            if (track != null)
            {
                favouritePlaylist.Tracks.Remove(track);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}

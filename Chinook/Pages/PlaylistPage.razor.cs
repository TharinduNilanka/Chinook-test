using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Chinook.Database.Repositories.Contract;
using Chinook.Database.Repositories;
using Chinook.Models;

namespace Chinook.Pages
{
    public partial class PlaylistPage
    {
        [Parameter] public long PlaylistId { get; set; }
        [Inject] IPlayListRepository PlayListRepository { get; set; }

        private Chinook.ClientModels.Playlist Playlist;
        private string InfoMessage;
        protected override async Task OnInitializedAsync()
        {
            await InvokeAsync(StateHasChanged);
            await LazyLoadingEventDataAsync();
        }
        protected override async Task OnParametersSetAsync()
        {
            await InvokeAsync(StateHasChanged);
            await LazyLoadingEventDataAsync();
        }
        private async Task LazyLoadingEventDataAsync()
        {
            await InvokeAsync(StateHasChanged);
            Playlist = await PlayListRepository.GetPlayListByIdAsync(PlaylistId);
        }
        private async Task FavoriteTrackAsync(long trackId)
        {
            var track = Playlist.Tracks.FirstOrDefault(t => t.TrackId == trackId);
            await PlayListRepository.AddTrackToPlayList(trackId, "Favorites");
            InfoMessage = $"Track {track.ArtistName} - {track.AlbumTitle} - {track.TrackName} added to playlist Favorites.";
            await LazyLoadingEventDataAsync();
            await InvokeAsync(StateHasChanged);
 
        }

        private async Task UnfavoriteTrackAsync(long trackId)
        {
            var track = Playlist.Tracks.FirstOrDefault(t => t.TrackId == trackId);
            await PlayListRepository.MarkTrackUnFavorite(trackId);
            InfoMessage = $"Track {track.ArtistName} - {track.AlbumTitle} - {track.TrackName} removed from playlist Favorites.";
            await LazyLoadingEventDataAsync();
            await InvokeAsync(StateHasChanged);
        }

        private void RemoveTrack(long trackId)
        {
            CloseInfoMessage();

        }

        private void CloseInfoMessage()
        {
            InfoMessage = "";
        }

    }
}

using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Shared.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Chinook.Database.Repositories;
using Chinook.Database.Repositories.Contract;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Chinook.Migrations;

namespace Chinook.Pages
{
    public partial class ArtistPage
    {
        [Parameter] public long ArtistId { get; set; }
        [Inject] IArtistRepository ArtistRepository { get; set; }
        [Inject] ITracksRepository TracksRepository { get; set; }
        [Inject] IPlayListRepository PlayListRepository { get; set; }
        [Inject] IUserPlayListRepository UserPlayListRepository { get; set; }
        private Modal PlaylistDialog { get; set; }
        private List<UserPlayList>? UserPlayLists;
        private Artist Artist;
        private List<PlaylistTrack> Tracks;
        private PlaylistTrack SelectedTrack;
        private string InfoMessage;
        private long selectedExistingPlaylist;
        private string newPlaylistName;

        protected override async Task OnInitializedAsync()
        {
            await InvokeAsync(StateHasChanged);
            await LazyLoadingEventDataAsync();
            UserPlayLists = await UserPlayListRepository.GetPlayListByUserIdAsync();
        }

        private async Task LazyLoadingEventDataAsync()
        {
            Artist = ArtistRepository.GetArtist(ArtistId);
            Tracks = await TracksRepository.GetTracksByArtistIdAsync(ArtistId);
        }
        private async Task FavoriteTrackAsync(long trackId)
        {
            var track = Tracks.FirstOrDefault(t => t.TrackId == trackId);
            await PlayListRepository.AddTrackToPlayList(trackId, "Favorites");
            InfoMessage = $"Track {track.ArtistName} - {track.AlbumTitle} - {track.TrackName} added to playlist Favorites.";
            await LazyLoadingEventDataAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task UnfavoriteTrackAsync(long trackId)
        {
            var track = Tracks.FirstOrDefault(t => t.TrackId == trackId);
            await PlayListRepository.MarkTrackUnFavorite(trackId);
            InfoMessage = $"Track {track.ArtistName} - {track.AlbumTitle} - {track.TrackName} removed from playlist Favorites.";
            await LazyLoadingEventDataAsync();
            await InvokeAsync(StateHasChanged);
        }

        private void OpenPlaylistDialog(long trackId)
        {
            CloseInfoMessage();
            SelectedTrack = Tracks.FirstOrDefault(t => t.TrackId == trackId);
            PlaylistDialog.Open();
        }

        private async void AddTrackToPlaylist()
        {
            CloseInfoMessage();
            InfoMessage = $"Track {Artist.Name} - {SelectedTrack.AlbumTitle} - {SelectedTrack.TrackName} added to playlist {{playlist name}}.";
            await PlayListRepository.AddTrackToPlayList(SelectedTrack.TrackId, newPlaylistName, selectedExistingPlaylist);
            PlaylistDialog.Close();
        }

        private void CloseInfoMessage()
        {
            InfoMessage = "";
        }
    }
}

using Chinook.Database.Repositories.Contract;
using Chinook.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Pages
{
    public partial class Index
    {
        private List<Artist> Artists; 
        private List<Artist> FilteredArtists;
        private string SearchQuary;
        [Inject] IArtistRepository ArtistRepository { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InvokeAsync(StateHasChanged);
            Artists = await ArtistRepository.GetArtists();
            UpdateFilteredArtists();
        }

        private void UpdateFilteredArtists()
        {
            if (string.IsNullOrWhiteSpace(SearchQuary))
            {
                FilteredArtists = Artists;
            }
            else
            {
                FilteredArtists = Artists.Where(a=>a.Name.Contains(SearchQuary,StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }
    }
}

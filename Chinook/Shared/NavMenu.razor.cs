using Chinook.ClientModels;
using Chinook.Database.Repositories;
using Chinook.Database.Repositories.Contract;
using Chinook.Models;
using Microsoft.AspNetCore.Components;

namespace Chinook.Shared
{
    public partial class NavMenu
    {
        [Inject] IUserPlayListRepository UserPlayListRepository { get; set; }
        [Inject] IPlayListRepository  PlayListRepository{ get; set; }

        private bool collapseNavMenu = true;
        private List<UserPlayList>? UserPlayLists { get; set; }

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        protected override async Task OnInitializedAsync()
        {
            await InvokeAsync(StateHasChanged);
            UserPlayLists = await UserPlayListRepository.GetPlayListByUserIdAsync();
            PlayListRepository.PlayListAdded += HandlePlaylistChanged;
        }
        //protected override async Task OnParametersSetAsync()
        //{
        //    await InvokeAsync(StateHasChanged);
        //    await LazyLoadingEventDataAsync();
        //}

        private async void HandlePlaylistChanged(object sender, EventArgs e)
        {
            UserPlayLists = await UserPlayListRepository.GetPlayListByUserIdAsync();
            StateHasChanged(); // Refresh the UI
        }

        private async Task LazyLoadingEventDataAsync(object sender, EventArgs e)
        {
            await InvokeAsync(StateHasChanged);
            
            StateHasChanged();
        }
        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}

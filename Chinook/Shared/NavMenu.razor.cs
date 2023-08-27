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

        private bool collapseNavMenu = true;
        [CascadingParameter] private List<UserPlayList>? UserPlayLists { get; set; }

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

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
            UserPlayLists = await UserPlayListRepository.GetPlayListByUserIdAsync();
        }
        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}

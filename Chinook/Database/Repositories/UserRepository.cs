using Chinook.Database.Repositories.Contract;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Chinook.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UserRepository(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }
        public async Task<string> GetUserId()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = (authState).User;
            return user.FindFirst(u => u.Type.Contains(ClaimTypes.NameIdentifier))?.Value;
        }

    }
}

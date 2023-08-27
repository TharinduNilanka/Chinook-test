using Chinook.ClientModels;
using Chinook.Database.Repositories.Contract;
using Chinook.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Chinook.Database.Repositories
{
    public class ArtistRepository : IArtistRepository
    {

        private readonly ChinookContext _dbContext;
       
        public ArtistRepository(ChinookContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Artist GetArtist(long ArtistId)
        {
            return _dbContext.Artists.SingleOrDefault(a => a.ArtistId == ArtistId);
        }
    }
}

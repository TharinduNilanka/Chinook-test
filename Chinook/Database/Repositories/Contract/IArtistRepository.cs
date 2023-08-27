using Chinook.Models;

namespace Chinook.Database.Repositories.Contract
{
    public interface IArtistRepository
    {
        Artist GetArtist(long ArtistId);
    }
}

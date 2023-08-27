using AutoMapper;
using Microsoft.DotNet.Scaffolding.Shared;
using PlaylistDto = Chinook.ClientModels.Playlist;
using Playlist = Chinook.Models.Playlist;
namespace Chinook.CustomConfiguration
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new(config =>
            {
                config.CreateMap<PlaylistDto, Playlist>();
            });
        }
    }
}

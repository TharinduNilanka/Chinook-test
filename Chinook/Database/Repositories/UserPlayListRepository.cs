using AutoMapper;
using Chinook.Database.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using UserPlayListDto = Chinook.ClientModels.UserPlayList;

namespace Chinook.Database.Repositories
{
    public class UserPlayListRepository : IUserPlayListRepository
    {
        private readonly ChinookContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserPlayListRepository(ChinookContext dbContext, IUserRepository userRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserPlayListDto>> GetPlayListByUserIdAsync()
        {
            var currentUserId = await _userRepository.GetUserId();
            return await _dbContext.UserPlaylists
                .Where(p => p.UserId == currentUserId)
                .Select(p => new UserPlayListDto()
                {
                    Name = p.Playlist.Name,
                    UserId = p.UserId,
                    PlaylistId = p.PlaylistId
                })
                .ToListAsync();
        }


        
    }
}

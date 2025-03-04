using Intercorp.Domain.Dtos;
using Intercorp.Domain.Repositories;

namespace Intercorp.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<IEnumerable<UserDto>> GetUsersAsync()
		{
			return await _userRepository.GetUsersAsync();
		}

		public async Task<UserPhotoDto> GetUserPhotoAsync(int id)
		{
			return await _userRepository.GetUserPhotoAsync(id);
		}
	}
}

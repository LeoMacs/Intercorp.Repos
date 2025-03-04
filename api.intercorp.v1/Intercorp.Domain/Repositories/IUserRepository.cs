using Intercorp.Domain.Dtos;

namespace Intercorp.Domain.Repositories
{
	public interface IUserRepository
	{
		Task<IEnumerable<UserDto>> GetUsersAsync();
		Task<UserPhotoDto> GetUserPhotoAsync(int id);
	}
}

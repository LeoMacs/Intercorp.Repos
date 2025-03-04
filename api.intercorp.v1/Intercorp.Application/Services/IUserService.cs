using Intercorp.Domain.Dtos;

namespace Intercorp.Application.Services
{
	public interface IUserService
	{
		Task<IEnumerable<UserDto>> GetUsersAsync();
		Task<UserPhotoDto> GetUserPhotoAsync(int id);
	}
}

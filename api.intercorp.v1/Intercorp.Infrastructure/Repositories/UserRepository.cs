using Intercorp.Domain.Dtos;
using Intercorp.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Xml.Linq;


namespace Intercorp.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		public UserRepository(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_configuration = configuration;
		}

		public async Task<IEnumerable<UserDto>> GetUsersAsync()
		{
			var response = await _httpClient.GetStringAsync(_configuration["ApiSettings:ReqresBaseUrl"]);
			var apiResponse = JsonConvert.DeserializeObject<UserApiResponse>(response);
			return apiResponse?.Data ?? new List<UserDto>();
		}

		public async Task<UserPhotoDto> GetUserPhotoAsync(int id)
		{
			var response = await _httpClient.GetStringAsync(_configuration["ApiSettings:ReqresBaseUrl"]);
			var apiResponse = JsonConvert.DeserializeObject<UserApiResponse>(response);

			var user = apiResponse?.Data.FirstOrDefault(u => u.Id == id);
			if (user == null) return new UserPhotoDto();

			var imageBytes = await _httpClient.GetByteArrayAsync(user.Avatar);
			return new UserPhotoDto { Id = id, AvatarBase64 = Convert.ToBase64String(imageBytes) };
		}
	}
}

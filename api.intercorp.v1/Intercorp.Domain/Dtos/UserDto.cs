namespace Intercorp.Domain.Dtos
{
	public class UserDto
	{
		public int Id { get; set; }
		public string FirstName { get; set; }=string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Avatar { get; set; } = string.Empty;
	}

	public class UserApiResponse
	{
		public List<UserDto> Data { get; set; }
	}
}

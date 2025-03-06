using Intercorp.Application.Services;
using Intercorp.Domain.Dtos;
using Intercorp.Domain.Repositories;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace Intercorp.Unitest.User.Application
{
	public class UserServiceUnitest
	{
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly UserService _userService;

		public UserServiceUnitest()
		{
			_userRepositoryMock = new Mock<IUserRepository>();
			_userService = new UserService(_userRepositoryMock.Object);
		}

		[Fact]
		public async Task GetUsersAsync_ReturnsListOfUsers()
		{
			// Arrange
			var fakeUsers = new List<UserDto>
		{
			new UserDto { Id = 1, FirstName = "Chimpandolfo", LastName = "Butircio", Email = "Chimpandolfo.Butircio@reqres.in", Avatar = "https://reqres.in/img/faces/1-image.jpg" },
			new UserDto { Id = 2, FirstName = "Debora", LastName = "Mesta", Email = "Debora.Mesta@reqres.in", Avatar = "https://reqres.in/img/faces/2-image.jpg" }
		};

			_userRepositoryMock.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(fakeUsers);

			// Act
			var users = await _userService.GetUsersAsync();

			// Assert
			Assert.NotNull(users);
			Assert.Equal(2, users.Count());
			Assert.Equal("Chimpandolfo", users.First().FirstName);
		}

		[Fact]
		public async Task GetUserPhotoAsync_ReturnsUserPhoto()
		{
			// Arrange
			var fakePhoto = new UserPhotoDto { Id = 1, AvatarBase64 = "base64imagestring" };

			_userRepositoryMock.Setup(repo => repo.GetUserPhotoAsync(1)).ReturnsAsync(fakePhoto);

			// Act
			var userPhoto = await _userService.GetUserPhotoAsync(1);

			// Assert
			Assert.NotNull(userPhoto);
			Assert.Equal(1, userPhoto.Id);
			Assert.Equal("base64imagestring", userPhoto.AvatarBase64);
		}
	}
}

using Intercorp.Api.Controllers;
using Intercorp.Application.Services;
using Intercorp.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace Intercorp.Unitest.User.Controllers
{
	public class UserControllerUnitest
	{
		private readonly Mock<IUserService> _userServiceMock;
		private readonly UserController _controller;

		public UserControllerUnitest()
		{
			_userServiceMock = new Mock<IUserService>();
			_controller = new UserController(_userServiceMock.Object);

		}

		//*******************************************************************/
		//Controlador para Obtener los usuarios
		//*******************************************************************/
		[Fact]
		public async Task Test_GetUsers_IfExiste()
		{
			// Arrange
			var mockUsers = new List<UserDto>
			{
				new UserDto
				{
					Id = 1,
					FirstName = "Chimpandolfo",
					LastName = "Butircio",
					Avatar = "xxxxxxxxxxx"
				}
			};

			_userServiceMock.Setup(s => s.GetUsersAsync()).ReturnsAsync(mockUsers);

			// Act
			var result = await _controller.GetUsers();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnedUsers = Assert.IsAssignableFrom<List<UserDto>>(okResult.Value);

			Assert.Single(returnedUsers);
			Assert.Equal("Chimpandolfo", returnedUsers[0].FirstName);

		}

		[Fact]
		public async Task GetUsersGetUsers_NoExiste()
		{
			List<UserDto> lista = new List<UserDto>();
			// Arrange
			_userServiceMock.Setup(s => s.GetUsersAsync()).ReturnsAsync(lista);

			// Act
			var result = await _controller.GetUsers();

			// Assert
			var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal("No users found", notFoundResult.Value);
		}
	}
}

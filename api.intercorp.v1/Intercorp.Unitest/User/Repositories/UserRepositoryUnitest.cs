using Intercorp.Domain.Dtos;
using Intercorp.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using Xunit;
using Assert = Xunit.Assert;

namespace Intercorp.Unitest.User.Repositories
{
	public class UserRepositoryUnitest
	{
		private readonly Mock<IConfiguration> _configurationMock;
		private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
		private readonly HttpClient _httpClient;
		private readonly UserRepository _userRepository;

		public UserRepositoryUnitest()
		{
			_configurationMock = new Mock<IConfiguration>();
			_configurationMock.Setup(config => config["ApiSettings:ReqresBaseUrl"])
							  .Returns("https://reqres.in/api/users");

			_httpMessageHandlerMock = new Mock<HttpMessageHandler>();

			_httpClient = new HttpClient(_httpMessageHandlerMock.Object)
			{
				BaseAddress = new System.Uri("https://reqres.in/api/")
			};

			_userRepository = new UserRepository(_httpClient, _configurationMock.Object);
		}

		[Fact]
		public async Task GetUsersAsync_ReturnsLista()
		{
			// Arrange
			var fakeResponse = new UserApiResponse
			{
				Data = new List<UserDto>
			{
				new UserDto { Id = 1, FirstName = "Chimpandolfo", LastName = "Butircio", Email = "Chimpandolfo.Butircio@reqres.in", Avatar = "https://reqres.in/img/faces/1-image.jpg" }
			}
			};

			var jsonResponse = JsonConvert.SerializeObject(fakeResponse);
			var httpResponse = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(jsonResponse)
			};

			_httpMessageHandlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(httpResponse);

			// Act
			var users = await _userRepository.GetUsersAsync();

			// Assert
			Assert.NotNull(users);
			Assert.Single(users);
			Assert.Equal(1, users.First().Id);
			Assert.Equal("Chimpandolfo", users.First().FirstName);
		}

		[Fact]
		public async Task GetUserPhotoAsync_ReturnsBase64Photo()
		{
			// Arrange
			var fakeResponse = new UserApiResponse
			{
				Data = new List<UserDto>
			{
				new UserDto { Id = 1, Avatar = "https://reqres.in/img/faces/1-image.jpg" }
			}
			};

			var jsonResponse = JsonConvert.SerializeObject(fakeResponse);
			var fakeImage = new byte[] { 1, 2, 3, 4 }; // Simulando una imagen

			var userResponse = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(jsonResponse)
			};

			var imageResponse = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new ByteArrayContent(fakeImage)
			};

			_httpMessageHandlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync",
					ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsoluteUri.Contains("users")),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(userResponse);

			_httpMessageHandlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync",
					ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsoluteUri.Contains("faces")),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(imageResponse);

			// Act
			var userPhoto = await _userRepository.GetUserPhotoAsync(1);

			// Assert
			Assert.NotNull(userPhoto);
			Assert.Equal(1, userPhoto.Id);
			Assert.Equal(Convert.ToBase64String(fakeImage), userPhoto.AvatarBase64);
		}
	}
}

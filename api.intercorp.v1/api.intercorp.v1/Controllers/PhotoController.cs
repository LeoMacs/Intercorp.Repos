using Intercorp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Intercorp.Api.Controllers
{
	[Route("api/photos")]
	[ApiController]
	public class PhotoController : ControllerBase
	{
		private readonly IUserService _userService;
		public PhotoController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("{id}")]
		[ResponseCache(Duration = 60)] // Rate Limiting
		public async Task<IActionResult> GetUserPhoto(int id)
		{
			var photo = await _userService.GetUserPhotoAsync(id);
			if (photo == null) return NotFound();
			return Ok(photo);
		}
	}
}

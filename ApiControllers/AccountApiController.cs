using Microsoft.AspNetCore.Mvc;
using ChessOnline.DTOs;
using ChessOnline.Services;

namespace ChessOnline.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
            => Ok(await _userService.RegisterAsync(dto));

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
            => Ok(await _userService.LoginAsync(dto));

        [HttpGet("profile/{id}")]
        public async Task<IActionResult> Profile(int id)
            => Ok(await _userService.GetProfileAsync(id));

        [HttpPost("upload-avatar/{id}")]
        public async Task<IActionResult> UploadAvatar(int id, IFormFile file)
            => Ok(await _userService.UploadAvatarAsync(id, file));
    }
}

//hihji
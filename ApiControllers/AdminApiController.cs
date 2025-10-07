using ChessOnline.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessOnline.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminApiController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminApiController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
            => Ok(await _adminService.GetAllUsersAsync());

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(int id)
            => Ok(await _adminService.GetUserByIdAsync(id));

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
            => Ok(await _adminService.DeleteUserAsync(id));

        [HttpPut("user/{id}/update-role")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] string role)
            => Ok(await _adminService.UpdateUserRoleAsync(id, role));
    }
}

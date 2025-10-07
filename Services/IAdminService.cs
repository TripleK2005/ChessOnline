using ChessOnline.Models;
using ChessOnline.DTOs.AccountDtos;
using ChessOnline.DTOs.AdminDtos;

namespace ChessOnline.Services
{
    public interface IAdminService
    {
        Task<List<ManageUserDto>> GetAllUsersAsync();
        Task<ApiResponse<ManageUserDto>> GetUserByIdAsync(int id);
        Task<ApiResponse<bool>> DeleteUserAsync(int id);
        Task<ApiResponse<User>> UpdateUserRoleAsync(int id, string newRole);
        Task<ApiResponse<bool>> BanUserAsync(int id);
        Task<ApiResponse<bool>> UnbanUserAsync(int id);
        Task<ApiResponse<User>> CreateUserAsync(ManageUserDto dto, string password);
    }
}

using System.Threading.Tasks;
using ChessOnline.DTOs.AccountDtos;
using ChessOnline.Models;

namespace ChessOnline.Services
{
    public interface IUserService
    {
        Task<ApiResponse<User>> RegisterAsync(UserRegisterDto dto);
        Task<ApiResponse<User>> LoginAsync(UserLoginDto dto);
        Task<ApiResponse<User>> GetProfileAsync(int userId);
        Task<ApiResponse<string>> UploadAvatarAsync(int userId, IFormFile file);
        Task<ApiResponse<string>> ChangePasswordAsync(ChangePasswordDto dto);
    }
}

using ChessOnline.Data;
using ChessOnline.DTOs.AccountDtos;
using ChessOnline.DTOs.AdminDtos;
using ChessOnline.Models;
using ChessOnline.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ChessOnline.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _hasher;

        public AdminService(AppDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<List<ManageUserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => u.Role != "Admin") // Không lấy admin
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new ManageUserDto
                {
                    UserID = u.UserID,
                    Username = u.Username,
                    Email = u.Email,
                    Avatar = u.Avatar,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt,
                    Status = u.Status // ✅ thêm trạng thái
                })
                .ToListAsync();
        }

        public async Task<ApiResponse<ManageUserDto>> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return ApiResponse<ManageUserDto>.Fail("User not found");

            var dto = new ManageUserDto
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Avatar = user.Avatar,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return ApiResponse<ManageUserDto>.Ok(dto);
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return ApiResponse<bool>.Fail("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true);
        }

        public async Task<ApiResponse<User>> UpdateUserRoleAsync(int id, string newRole)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return ApiResponse<User>.Fail("User not found");

            user.Role = newRole;
            await _context.SaveChangesAsync();
            return ApiResponse<User>.Ok(user);
        }

        public async Task<ApiResponse<User>> CreateUserAsync(ManageUserDto dto, string password)
        {
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Avatar = dto.Avatar,
                Role = dto.Role,
                PasswordHash = _hasher.HashPassword(null, password),
                Status = AccountStatus.Active
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return ApiResponse<User>.Ok(user, "User created successfully");
        }


        // ✅ Thêm: Khóa tài khoản
        public async Task<ApiResponse<bool>> BanUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return ApiResponse<bool>.Fail("User not found");

            user.Status = AccountStatus.Banned;
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "User has been banned.");
        }

        // ✅ Thêm: Mở khóa tài khoản
        public async Task<ApiResponse<bool>> UnbanUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return ApiResponse<bool>.Fail("User not found");

            user.Status = AccountStatus.Active;
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "User has been reactivated.");
        }
    }
}

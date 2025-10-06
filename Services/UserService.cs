using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChessOnline.Data;
using ChessOnline.DTOs;
using ChessOnline.Models;

namespace ChessOnline.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _hasher;
        private readonly IWebHostEnvironment _env;

        public UserService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
            _env = env;
        }

        public async Task<ApiResponse<User>> RegisterAsync(UserRegisterDto dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
                return ApiResponse<User>.Fail("Username already exists");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = _hasher.HashPassword(null, dto.Password),
                Role = "Player",
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.PasswordHash = null; // tránh leak hash
            return ApiResponse<User>.Ok(user, "Register success");
        }

        public async Task<ApiResponse<User>> LoginAsync(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
                return ApiResponse<User>.Fail("User not found");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result != PasswordVerificationResult.Success)
                return ApiResponse<User>.Fail("Invalid password");

            user.PasswordHash = null;
            return ApiResponse<User>.Ok(user, "Login successful");
        }

        public async Task<ApiResponse<User>> GetProfileAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return ApiResponse<User>.Fail("User not found");

            user.PasswordHash = null;
            return ApiResponse<User>.Ok(user);
        }

        public async Task<ApiResponse<string>> UploadAvatarAsync(int userId, IFormFile file)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return ApiResponse<string>.Fail("User not found");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            user.Avatar = $"/uploads/avatars/{fileName}";
            await _context.SaveChangesAsync();

            return ApiResponse<string>.Ok(user.Avatar, "Avatar uploaded");
        }

        public async Task<ApiResponse<string>> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return ApiResponse<string>.Fail("User not found");

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword);
            if (verify != PasswordVerificationResult.Success)
                return ApiResponse<string>.Fail("Old password is incorrect");

            user.PasswordHash = _hasher.HashPassword(user, dto.NewPassword);
            await _context.SaveChangesAsync();

            return ApiResponse<string>.Ok(null, "Password updated successfully");
        }

    }
}

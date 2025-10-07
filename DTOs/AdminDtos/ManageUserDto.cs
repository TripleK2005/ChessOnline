using ChessOnline.Models.Enums;

namespace ChessOnline.DTOs.AdminDtos
{
    public class ManageUserDto
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string Role { get; set; } = "Player";
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public AccountStatus Status { get; set; } = AccountStatus.Active;
    }
}

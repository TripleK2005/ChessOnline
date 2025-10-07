using System.ComponentModel.DataAnnotations;

namespace ChessOnline.Models.Admin
{
    public class CreateUserViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        public string Role { get; set; } = "Player";
        public string? Avatar { get; set; }
    }
}

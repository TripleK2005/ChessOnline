using System.ComponentModel.DataAnnotations;

namespace ChessOnline.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
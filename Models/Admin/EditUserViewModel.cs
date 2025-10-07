using System.ComponentModel.DataAnnotations;

namespace ChessOnline.Models.Admin
{
    public class EditUserViewModel
    {
        public int UserID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }
    }
}

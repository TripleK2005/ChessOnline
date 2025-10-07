using ChessOnline.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChessOnline.Models.Admin
{
    public class ManageUsersViewModel
    {
        public int UserID { get; set; }

        [Display(Name = "Tên ??ng nh?p")]
        public string Username { get; set; }

        [Display(Name = "Email ng??i dùng")]
        public string Email { get; set; }

        public string Role { get; set; }

        public string Avatar { get; set; }

        public string CreatedAtFormatted => CreatedAt.ToString("yyyy-MM-dd");

        public DateTime CreatedAt { get; set; }

        public AccountStatus Status { get; set; }
    }
}

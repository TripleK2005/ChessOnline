using ChessOnline.Models.Enums;
using System;
using System.Collections.Generic;
using ChessOnline.Models.Games;

namespace ChessOnline.Models
{
    public class User
    {
        public int UserID { get; set; } // PK
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
        public string Role { get; set; } = "Player"; // Player / Admin
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public AccountStatus Status { get; set; } = AccountStatus.Active;

        // Navigation
        public ICollection<Game> GamesAsWhite { get; set; }
        public ICollection<Game> GamesAsBlack { get; set; }
        public ICollection<Move> Moves { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<Report> ReportsMade { get; set; }
        public ICollection<Report> ReportsReceived { get; set; }
    }
}

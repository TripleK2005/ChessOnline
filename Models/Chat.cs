using System;
using ChessOnline.Models.Games;

namespace ChessOnline.Models
{
    public class Chat
    {
        public int ChatID { get; set; }
        public int GameID { get; set; }   // FK
        public int UserID { get; set; }   // FK

        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public Game Game { get; set; } // Fix: Use fully qualified type if 'Game' is a namespace

        public User User { get; set; }
    }
}

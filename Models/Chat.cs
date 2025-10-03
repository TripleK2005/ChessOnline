using System;

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
        public Game Game { get; set; }
        public User User { get; set; }
    }
}

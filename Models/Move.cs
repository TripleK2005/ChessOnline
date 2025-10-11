using System;

namespace ChessOnline.Models.Games
{
    public class Move
    {
        public int MoveID { get; set; } // PK
        public int GameID { get; set; } // FK
        public int PlayerID { get; set; } // FK
        public string FromSquare { get; set; } // ví dụ: "e2"
        public string ToSquare { get; set; } // ví dụ: "e4"
        public string? Promotion { get; set; } // ví dụ: "Queen"
        public int MoveNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Game Game { get; set; }
        public User Player { get; set; }
    }
}

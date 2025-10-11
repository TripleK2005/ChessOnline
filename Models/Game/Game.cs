using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChessOnline.Models.Games
{
    public class Game
    {
        public int GameID { get; set; } // PK
        public string Code { get; set; } // Mã trận (6 ký tự)
        public int WhitePlayerID { get; set; } // FK
        public int BlackPlayerID { get; set; } // FK
        public string Status { get; set; } = "Waiting"; // Waiting, Playing, Finished
        public string? Result { get; set; } // WhiteWin, BlackWin, Draw
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Game() { }
        public User WhitePlayer { get; set; }
        public User BlackPlayer { get; set; }
        public ICollection<Move> Moves { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}

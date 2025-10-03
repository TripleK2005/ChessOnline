using System;

namespace ChessOnline.Models
{
    public class Report
    {
        public int ReportID { get; set; } // PK
        public int ReporterID { get; set; } // FK
        public int ReportedUserID { get; set; } // FK
        public int? GameID { get; set; } // FK (optional)
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public User Reporter { get; set; }
        public User ReportedUser { get; set; }
        public Game Game { get; set; }
    }
}

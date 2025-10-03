using Microsoft.EntityFrameworkCore;
using ChessOnline.Models;

namespace ChessOnline.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Game - WhitePlayer (User)
            modelBuilder.Entity<Game>()
                .HasOne(g => g.WhitePlayer)
                .WithMany(u => u.GamesAsWhite)
                .HasForeignKey(g => g.WhitePlayerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Game - BlackPlayer (User)
            modelBuilder.Entity<Game>()
                .HasOne(g => g.BlackPlayer)
                .WithMany(u => u.GamesAsBlack)
                .HasForeignKey(g => g.BlackPlayerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Move - Game
            modelBuilder.Entity<Move>()
                .HasOne(m => m.Game)
                .WithMany(g => g.Moves)
                .HasForeignKey(m => m.GameID)
                .OnDelete(DeleteBehavior.Cascade);

            // Move - Player (User)
            modelBuilder.Entity<Move>()
                .HasOne(m => m.Player)
                .WithMany(u => u.Moves)
                .HasForeignKey(m => m.PlayerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat - Game
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Game)
                .WithMany(g => g.Chats)
                .HasForeignKey(c => c.GameID)
                .OnDelete(DeleteBehavior.Cascade);

            // Chat - User
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.User)
                .WithMany(u => u.Chats)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Report - Reporter (User)
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany(u => u.ReportsMade)
                .HasForeignKey(r => r.ReporterID)
                .OnDelete(DeleteBehavior.Restrict);

            // Report - ReportedUser (User)
            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReportedUser)
                .WithMany(u => u.ReportsReceived)
                .HasForeignKey(r => r.ReportedUserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Report - Game (optional)
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Game)
                .WithMany(g => g.Reports)
                .HasForeignKey(r => r.GameID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

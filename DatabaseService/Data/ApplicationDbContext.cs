using Microsoft.EntityFrameworkCore;
using DatabaseService.Models;

namespace DatabaseService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Server>().HasKey(s => s.Id);
            modelBuilder.Entity<Playlist>().HasKey(p => p.Id);
            modelBuilder.Entity<Track>().HasKey(t => t.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Servers)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Server>()
                .HasMany(s => s.Playlists)
                .WithOne(p => p.Server)
                .HasForeignKey(p => p.ServerId);

            modelBuilder.Entity<Playlist>()
                .HasMany(p => p.Tracks)
                .WithOne(t => t.Playlist)
                .HasForeignKey(t => t.PlaylistId);
        }
    }
}

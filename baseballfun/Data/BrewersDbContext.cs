using baseballfun.Models;
using Microsoft.EntityFrameworkCore;

namespace baseballfun.Data;

public class BrewersDbContext(DbContextOptions<BrewersDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players => Set<Player>();
    public DbSet<PlayerStat> PlayerStats => Set<PlayerStat>();
    public DbSet<Game> Games => Set<Game>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(100).IsRequired();
            // Enums are stored as text so the table is readable in SQL tools.
            e.Property(p => p.Position).HasConversion<string>().HasMaxLength(20);
            e.Property(p => p.Bats).HasConversion<string>().HasMaxLength(10);
            e.Property(p => p.Throws).HasConversion<string>().HasMaxLength(10);
            e.HasMany(p => p.Stats)
                .WithOne()
                .HasForeignKey(s => s.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PlayerStat>(e =>
        {
            e.Property(s => s.InningsPitched).HasPrecision(5, 1);
            e.HasIndex(s => new { s.PlayerId, s.Season }).IsUnique();
        });

        modelBuilder.Entity<Game>(e =>
        {
            e.Property(g => g.Opponent).HasMaxLength(50).IsRequired();
            e.HasIndex(g => g.GameDate);
        });
    }
}

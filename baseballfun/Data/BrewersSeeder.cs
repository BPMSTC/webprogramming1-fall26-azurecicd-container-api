using System.Data;
using baseballfun.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace baseballfun.Data;

public static class BrewersSeeder
{
    /// <summary>
    /// Inserts the Brewers mock data into any table that is still empty. Safe to call on every startup,
    /// and safe when several API instances start at the same time (an application lock serializes them).
    /// </summary>
    public static async Task SeedAsync(BrewersDbContext db, ILogger logger, CancellationToken ct = default)
    {
        var strategy = db.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            db.ChangeTracker.Clear();
            await using var tx = await db.Database.BeginTransactionAsync(ct);

            var lockResult = new SqlParameter("@result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            await db.Database.ExecuteSqlRawAsync(
                "EXEC @result = sp_getapplock @Resource = N'BrewersSeed', @LockMode = N'Exclusive', @LockOwner = N'Transaction', @LockTimeout = 30000",
                [lockResult],
                ct);
            if ((int)lockResult.Value < 0)
            {
                throw new InvalidOperationException($"Could not acquire the seeding lock (sp_getapplock returned {lockResult.Value}).");
            }

            if (!await db.Players.AnyAsync(ct))
            {
                var players = BrewersSeedData.CreatePlayers();
                db.Players.AddRange(players);
                await db.SaveChangesAsync(ct);
                logger.LogInformation("Seeded {PlayerCount} players with {StatCount} stat rows",
                    players.Count, players.Sum(p => p.Stats.Count));
            }
            else
            {
                logger.LogInformation("Players table already has data; skipping player seed");
            }

            if (!await db.Games.AnyAsync(ct))
            {
                var games = BrewersSeedData.CreateGames();
                db.Games.AddRange(games);
                await db.SaveChangesAsync(ct);
                logger.LogInformation("Seeded {GameCount} games", games.Count);
            }
            else
            {
                logger.LogInformation("Games table already has data; skipping game seed");
            }

            await tx.CommitAsync(ct);
        });
    }
}

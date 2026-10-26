using baseballfun.Data;
using baseballfun.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace baseballfun.Repositories;

public class SqlPlayerRepository(BrewersDbContext db) : IPlayerRepository
{
    public async Task<IReadOnlyList<Player>> GetAllAsync(Position? position = null, CancellationToken ct = default) =>
        await db.Players.AsNoTracking()
            .Where(p => position == null || p.Position == position)
            .OrderBy(p => p.Number)
            .ThenBy(p => p.Id)
            .ToListAsync(ct);

    public Task<Player?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Players.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Player> AddAsync(Player player, CancellationToken ct = default)
    {
        player.Id = 0;
        db.Players.Add(player);
        await db.SaveChangesAsync(ct);
        return player;
    }

    public async Task<bool> UpdateAsync(Player player, CancellationToken ct = default)
    {
        var existing = await db.Players.FirstOrDefaultAsync(p => p.Id == player.Id, ct);
        if (existing is null)
        {
            return false;
        }

        existing.Name = player.Name;
        existing.Number = player.Number;
        existing.Position = player.Position;
        existing.Bats = player.Bats;
        existing.Throws = player.Throws;
        existing.BirthDate = player.BirthDate;
        await db.SaveChangesAsync(ct);
        return true;
    }

    // Stats are removed by the cascading foreign key.
    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
        await db.Players.Where(p => p.Id == id).ExecuteDeleteAsync(ct) > 0;

    public async Task<IReadOnlyList<PlayerStat>?> GetStatsAsync(int playerId, CancellationToken ct = default)
    {
        if (!await db.Players.AnyAsync(p => p.Id == playerId, ct))
        {
            return null;
        }

        return await db.PlayerStats.AsNoTracking()
            .Where(s => s.PlayerId == playerId)
            .OrderBy(s => s.Season)
            .ToListAsync(ct);
    }

    public async Task<PlayerStat?> AddStatAsync(int playerId, PlayerStat stat, CancellationToken ct = default)
    {
        if (!await db.Players.AnyAsync(p => p.Id == playerId, ct))
        {
            return null;
        }

        stat.Id = 0;
        stat.PlayerId = playerId;
        db.PlayerStats.Add(stat);

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            // The unique (PlayerId, Season) index is what makes this check safe under concurrent requests.
            throw new DuplicateStatException(playerId, stat.Season);
        }

        return stat;
    }
}

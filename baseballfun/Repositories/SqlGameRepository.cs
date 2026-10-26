using baseballfun.Data;
using baseballfun.Models;
using Microsoft.EntityFrameworkCore;

namespace baseballfun.Repositories;

public class SqlGameRepository(BrewersDbContext db) : IGameRepository
{
    public async Task<IReadOnlyList<Game>> GetAllAsync(string? opponent = null, CancellationToken ct = default)
    {
        var query = db.Games.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(opponent))
        {
            // Parameterized by EF: the value is never concatenated into the SQL text.
            query = query.Where(g => g.Opponent.Contains(opponent));
        }

        return await query.OrderBy(g => g.GameDate).ThenBy(g => g.Id).ToListAsync(ct);
    }

    public Task<Game?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id, ct);

    public async Task<Game> AddAsync(Game game, CancellationToken ct = default)
    {
        game.Id = 0;
        db.Games.Add(game);
        await db.SaveChangesAsync(ct);
        return game;
    }

    public async Task<bool> UpdateAsync(Game game, CancellationToken ct = default)
    {
        var existing = await db.Games.FirstOrDefaultAsync(g => g.Id == game.Id, ct);
        if (existing is null)
        {
            return false;
        }

        existing.GameDate = game.GameDate;
        existing.Opponent = game.Opponent;
        existing.IsHome = game.IsHome;
        existing.BrewersRuns = game.BrewersRuns;
        existing.OpponentRuns = game.OpponentRuns;
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
        await db.Games.Where(g => g.Id == id).ExecuteDeleteAsync(ct) > 0;
}

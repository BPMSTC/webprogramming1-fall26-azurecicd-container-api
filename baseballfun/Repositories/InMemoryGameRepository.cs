using baseballfun.Data;
using baseballfun.Models;

namespace baseballfun.Repositories;

public class InMemoryGameRepository : IGameRepository
{
    private readonly Lock _lock = new();
    private readonly Dictionary<int, Game> _games = [];
    private int _nextId = 1;

    public InMemoryGameRepository()
    {
        foreach (var game in BrewersSeedData.CreateGames())
        {
            game.Id = _nextId++;
            _games[game.Id] = game;
        }
    }

    public Task<IReadOnlyList<Game>> GetAllAsync(string? opponent = null, CancellationToken ct = default)
    {
        lock (_lock)
        {
            IReadOnlyList<Game> result = _games.Values
                .Where(g => string.IsNullOrWhiteSpace(opponent)
                    || g.Opponent.Contains(opponent, StringComparison.OrdinalIgnoreCase))
                .OrderBy(g => g.GameDate)
                .ThenBy(g => g.Id)
                .Select(g => g.Clone())
                .ToList();
            return Task.FromResult(result);
        }
    }

    public Task<Game?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_games.TryGetValue(id, out var game) ? game.Clone() : null);
        }
    }

    public Task<Game> AddAsync(Game game, CancellationToken ct = default)
    {
        lock (_lock)
        {
            var stored = game.Clone();
            stored.Id = _nextId++;
            _games[stored.Id] = stored;
            return Task.FromResult(stored.Clone());
        }
    }

    public Task<bool> UpdateAsync(Game game, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (!_games.ContainsKey(game.Id))
            {
                return Task.FromResult(false);
            }

            _games[game.Id] = game.Clone();
            return Task.FromResult(true);
        }
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_games.Remove(id));
        }
    }
}

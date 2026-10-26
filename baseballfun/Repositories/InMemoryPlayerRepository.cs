using baseballfun.Data;
using baseballfun.Models;

namespace baseballfun.Repositories;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly Lock _lock = new();
    private readonly Dictionary<int, Player> _players = [];
    private int _nextPlayerId = 1;
    private int _nextStatId = 1;

    public InMemoryPlayerRepository()
    {
        foreach (var player in BrewersSeedData.CreatePlayers())
        {
            Store(player);
        }
    }

    public Task<IReadOnlyList<Player>> GetAllAsync(Position? position = null, CancellationToken ct = default)
    {
        lock (_lock)
        {
            IReadOnlyList<Player> result = _players.Values
                .Where(p => position is null || p.Position == position)
                .OrderBy(p => p.Number)
                .Select(Clone)
                .ToList();
            return Task.FromResult(result);
        }
    }

    public Task<Player?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_players.TryGetValue(id, out var player) ? Clone(player) : null);
        }
    }

    public Task<Player> AddAsync(Player player, CancellationToken ct = default)
    {
        lock (_lock)
        {
            return Task.FromResult(Clone(Store(Clone(player))));
        }
    }

    public Task<bool> UpdateAsync(Player player, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (!_players.TryGetValue(player.Id, out var existing))
            {
                return Task.FromResult(false);
            }

            existing.Name = player.Name;
            existing.Number = player.Number;
            existing.Position = player.Position;
            existing.Bats = player.Bats;
            existing.Throws = player.Throws;
            existing.BirthDate = player.BirthDate;
            return Task.FromResult(true);
        }
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_players.Remove(id));
        }
    }

    public Task<IReadOnlyList<PlayerStat>?> GetStatsAsync(int playerId, CancellationToken ct = default)
    {
        lock (_lock)
        {
            IReadOnlyList<PlayerStat>? stats = _players.TryGetValue(playerId, out var player)
                ? player.Stats.OrderBy(s => s.Season).Select(s => s.Clone()).ToList()
                : null;
            return Task.FromResult(stats);
        }
    }

    public Task<PlayerStat?> AddStatAsync(int playerId, PlayerStat stat, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (!_players.TryGetValue(playerId, out var player))
            {
                return Task.FromResult<PlayerStat?>(null);
            }

            if (player.Stats.Any(s => s.Season == stat.Season))
            {
                throw new DuplicateStatException(playerId, stat.Season);
            }

            var stored = stat.Clone();
            stored.Id = _nextStatId++;
            stored.PlayerId = playerId;
            player.Stats.Add(stored);
            return Task.FromResult<PlayerStat?>(stored.Clone());
        }
    }

    // Callers must hold _lock.
    private Player Store(Player player)
    {
        player.Id = _nextPlayerId++;
        foreach (var stat in player.Stats)
        {
            stat.Id = _nextStatId++;
            stat.PlayerId = player.Id;
        }

        _players[player.Id] = player;
        return player;
    }

    private static Player Clone(Player player) => new()
    {
        Id = player.Id,
        Name = player.Name,
        Number = player.Number,
        Position = player.Position,
        Bats = player.Bats,
        Throws = player.Throws,
        BirthDate = player.BirthDate,
        Stats = player.Stats.Select(s => s.Clone()).ToList()
    };
}

using baseballfun.Models;

namespace baseballfun.Repositories;

public interface IPlayerRepository
{
    Task<IReadOnlyList<Player>> GetAllAsync(Position? position = null, CancellationToken ct = default);
    Task<Player?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Player> AddAsync(Player player, CancellationToken ct = default);

    /// <summary>Updates the player's own fields (not stats). Returns false if the player does not exist.</summary>
    Task<bool> UpdateAsync(Player player, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);

    /// <summary>Returns null if the player does not exist.</summary>
    Task<IReadOnlyList<PlayerStat>?> GetStatsAsync(int playerId, CancellationToken ct = default);

    /// <summary>Returns null if the player does not exist. Throws <see cref="DuplicateStatException"/> if the season already has stats.</summary>
    Task<PlayerStat?> AddStatAsync(int playerId, PlayerStat stat, CancellationToken ct = default);
}

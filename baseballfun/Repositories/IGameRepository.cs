using baseballfun.Models;

namespace baseballfun.Repositories;

public interface IGameRepository
{
    Task<IReadOnlyList<Game>> GetAllAsync(string? opponent = null, CancellationToken ct = default);
    Task<Game?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Game> AddAsync(Game game, CancellationToken ct = default);
    Task<bool> UpdateAsync(Game game, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}

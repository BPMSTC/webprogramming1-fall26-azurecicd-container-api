namespace baseballfun.Repositories;

public class DuplicateStatException(int playerId, int season)
    : Exception($"Player {playerId} already has stats for season {season}.")
{
    public int PlayerId { get; } = playerId;
    public int Season { get; } = season;
}

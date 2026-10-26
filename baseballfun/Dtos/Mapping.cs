using baseballfun.Models;

namespace baseballfun.Dtos;

public static class Mapping
{
    public static PlayerResponse ToResponse(this Player p) =>
        new(p.Id, p.Name, p.Number, p.Position, p.Bats, p.Throws, p.BirthDate);

    public static Player ToEntity(this PlayerRequest r, int id = 0) => new()
    {
        Id = id,
        Name = r.Name.Trim(),
        Number = r.Number,
        Position = r.Position,
        Bats = r.Bats,
        Throws = r.Throws,
        BirthDate = r.BirthDate
    };

    public static PlayerStatResponse ToResponse(this PlayerStat s) => new(
        s.Id, s.PlayerId, s.Season, s.AtBats, s.Hits, s.HomeRuns, s.RunsBattedIn, s.StolenBases,
        s.InningsPitched, s.Wins, s.Losses, s.Saves, s.Strikeouts, s.EarnedRuns);

    public static PlayerStat ToEntity(this PlayerStatRequest r) => new()
    {
        Season = r.Season,
        AtBats = r.AtBats,
        Hits = r.Hits,
        HomeRuns = r.HomeRuns,
        RunsBattedIn = r.RunsBattedIn,
        StolenBases = r.StolenBases,
        InningsPitched = r.InningsPitched,
        Wins = r.Wins,
        Losses = r.Losses,
        Saves = r.Saves,
        Strikeouts = r.Strikeouts,
        EarnedRuns = r.EarnedRuns
    };

    public static GameResponse ToResponse(this Game g) => new(
        g.Id, g.GameDate, g.Opponent, g.IsHome, g.BrewersRuns, g.OpponentRuns,
        g.BrewersRuns is null || g.OpponentRuns is null ? null : g.BrewersRuns > g.OpponentRuns ? "W" : "L");

    public static Game ToEntity(this GameRequest r, int id = 0) => new()
    {
        Id = id,
        GameDate = r.GameDate,
        Opponent = r.Opponent.Trim(),
        IsHome = r.IsHome,
        BrewersRuns = r.BrewersRuns,
        OpponentRuns = r.OpponentRuns
    };
}

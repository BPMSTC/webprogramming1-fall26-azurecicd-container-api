using System.ComponentModel.DataAnnotations;
using baseballfun.Models;

namespace baseballfun.Dtos;

public record PlayerResponse(
    int Id,
    string Name,
    int Number,
    Position Position,
    BattingSide Bats,
    ThrowingHand Throws,
    DateOnly? BirthDate);

public class PlayerRequest
{
    [Required, StringLength(100, MinimumLength = 1)]
    public required string Name { get; init; }

    [Range(0, 99)]
    public required int Number { get; init; }

    [EnumDataType(typeof(Position))]
    public required Position Position { get; init; }

    [EnumDataType(typeof(BattingSide))]
    public required BattingSide Bats { get; init; }

    [EnumDataType(typeof(ThrowingHand))]
    public required ThrowingHand Throws { get; init; }

    public DateOnly? BirthDate { get; init; }
}

public record PlayerStatResponse(
    int Id,
    int PlayerId,
    int Season,
    int? AtBats,
    int? Hits,
    int? HomeRuns,
    int? RunsBattedIn,
    int? StolenBases,
    decimal? InningsPitched,
    int? Wins,
    int? Losses,
    int? Saves,
    int? Strikeouts,
    int? EarnedRuns);

public class PlayerStatRequest
{
    [Range(1900, 2100)]
    public required int Season { get; init; }

    [Range(0, 1000)] public int? AtBats { get; init; }
    [Range(0, 1000)] public int? Hits { get; init; }
    [Range(0, 100)] public int? HomeRuns { get; init; }
    [Range(0, 300)] public int? RunsBattedIn { get; init; }
    [Range(0, 200)] public int? StolenBases { get; init; }

    [Range(0, 500)] public decimal? InningsPitched { get; init; }
    [Range(0, 50)] public int? Wins { get; init; }
    [Range(0, 50)] public int? Losses { get; init; }
    [Range(0, 80)] public int? Saves { get; init; }
    [Range(0, 600)] public int? Strikeouts { get; init; }
    [Range(0, 300)] public int? EarnedRuns { get; init; }
}

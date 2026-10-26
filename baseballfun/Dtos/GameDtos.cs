using System.ComponentModel.DataAnnotations;

namespace baseballfun.Dtos;

public record GameResponse(
    int Id,
    DateOnly GameDate,
    string Opponent,
    bool IsHome,
    int? BrewersRuns,
    int? OpponentRuns,
    string? Result);

public class GameRequest : IValidatableObject
{
    public required DateOnly GameDate { get; init; }

    [Required, StringLength(50, MinimumLength = 1)]
    public required string Opponent { get; init; }

    public required bool IsHome { get; init; }

    [Range(0, 99)]
    public int? BrewersRuns { get; init; }

    [Range(0, 99)]
    public int? OpponentRuns { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (BrewersRuns.HasValue != OpponentRuns.HasValue)
        {
            yield return new ValidationResult(
                "BrewersRuns and OpponentRuns must both be provided or both be omitted.",
                [nameof(BrewersRuns), nameof(OpponentRuns)]);
        }
        else if (BrewersRuns.HasValue && BrewersRuns == OpponentRuns)
        {
            yield return new ValidationResult(
                "A completed baseball game cannot end in a tie.",
                [nameof(BrewersRuns), nameof(OpponentRuns)]);
        }
    }
}

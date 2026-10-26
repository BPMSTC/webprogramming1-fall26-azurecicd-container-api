namespace baseballfun.Models;

public class Game
{
    public int Id { get; set; }
    public DateOnly GameDate { get; set; }
    public string Opponent { get; set; } = string.Empty;
    public bool IsHome { get; set; }

    // Both scores are null until the game has been played.
    public int? BrewersRuns { get; set; }
    public int? OpponentRuns { get; set; }

    public Game Clone() => (Game)MemberwiseClone();
}

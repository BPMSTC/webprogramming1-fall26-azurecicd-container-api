namespace baseballfun.Models;

// One row per player per season. Batting fields are null for pure pitchers and pitching fields are null for hitters.
public class PlayerStat
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int Season { get; set; }

    public int? AtBats { get; set; }
    public int? Hits { get; set; }
    public int? HomeRuns { get; set; }
    public int? RunsBattedIn { get; set; }
    public int? StolenBases { get; set; }

    public decimal? InningsPitched { get; set; }
    public int? Wins { get; set; }
    public int? Losses { get; set; }
    public int? Saves { get; set; }
    public int? Strikeouts { get; set; }
    public int? EarnedRuns { get; set; }

    public PlayerStat Clone() => (PlayerStat)MemberwiseClone();
}

using baseballfun.Models;

namespace baseballfun.Data;

// Mock data only: rosters, numbers and stat lines are illustrative, not real records.
// Ids are left unset so each storage implementation assigns its own.
public static class BrewersSeedData
{
    public static List<Player> CreatePlayers() =>
    [
        Hitter("Christian Yelich", 22, Position.LeftField, BattingSide.Left, ThrowingHand.Right, 540, 148, 22, 78, 18),
        Hitter("William Contreras", 24, Position.Catcher, BattingSide.Right, ThrowingHand.Right, 585, 165, 24, 92, 6),
        Hitter("Jackson Chourio", 11, Position.CenterField, BattingSide.Right, ThrowingHand.Right, 560, 156, 28, 88, 21),
        Hitter("Brice Turang", 2, Position.SecondBase, BattingSide.Left, ThrowingHand.Right, 600, 170, 12, 66, 35),
        Hitter("Joey Ortiz", 3, Position.ThirdBase, BattingSide.Right, ThrowingHand.Right, 520, 132, 14, 61, 9),
        Hitter("Sal Frelick", 10, Position.RightField, BattingSide.Left, ThrowingHand.Left, 495, 138, 8, 52, 17),
        Pitcher("Freddy Peralta", 51, 176.1m, 17, 6, 0, 204, 62),
        Pitcher("Jacob Misiorowski", 32, 142.0m, 10, 8, 0, 188, 58),
        Pitcher("Trevor Megill", 29, 64.2m, 3, 4, 30, 72, 22)
    ];

    public static List<Game> CreateGames() =>
    [
        new() { GameDate = new(2026, 4, 3), Opponent = "Chicago Cubs", IsHome = true, BrewersRuns = 5, OpponentRuns = 3 },
        new() { GameDate = new(2026, 4, 4), Opponent = "Chicago Cubs", IsHome = true, BrewersRuns = 2, OpponentRuns = 4 },
        new() { GameDate = new(2026, 4, 12), Opponent = "St. Louis Cardinals", IsHome = false, BrewersRuns = 6, OpponentRuns = 1 },
        new() { GameDate = new(2026, 4, 13), Opponent = "St. Louis Cardinals", IsHome = false, BrewersRuns = 3, OpponentRuns = 2 },
        new() { GameDate = new(2026, 5, 2), Opponent = "Cincinnati Reds", IsHome = true, BrewersRuns = 7, OpponentRuns = 5 },
        new() { GameDate = new(2026, 5, 3), Opponent = "Pittsburgh Pirates", IsHome = true }
    ];

    private static Player Hitter(string name, int number, Position position, BattingSide bats, ThrowingHand throws,
        int atBats, int hits, int homeRuns, int rbi, int stolenBases) => new()
    {
        Name = name,
        Number = number,
        Position = position,
        Bats = bats,
        Throws = throws,
        Stats =
        [
            new()
            {
                Season = 2025,
                AtBats = atBats,
                Hits = hits,
                HomeRuns = homeRuns,
                RunsBattedIn = rbi,
                StolenBases = stolenBases
            }
        ]
    };

    private static Player Pitcher(string name, int number, decimal innings, int wins, int losses, int saves,
        int strikeouts, int earnedRuns) => new()
    {
        Name = name,
        Number = number,
        Position = Position.Pitcher,
        Bats = BattingSide.Right,
        Throws = ThrowingHand.Right,
        Stats =
        [
            new()
            {
                Season = 2025,
                InningsPitched = innings,
                Wins = wins,
                Losses = losses,
                Saves = saves,
                Strikeouts = strikeouts,
                EarnedRuns = earnedRuns
            }
        ]
    };
}

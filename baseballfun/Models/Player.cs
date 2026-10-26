namespace baseballfun.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Number { get; set; }
    public Position Position { get; set; }
    public BattingSide Bats { get; set; }
    public ThrowingHand Throws { get; set; }
    public DateOnly? BirthDate { get; set; }
    public List<PlayerStat> Stats { get; set; } = [];
}

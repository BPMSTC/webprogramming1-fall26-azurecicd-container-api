using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace baseballfun.Data;

// Used only by `dotnet ef` tooling. Set ConnectionStrings__BrewersDb to point `database update` at a real server;
// without it the placeholder is enough for `migrations add`, which never connects.
public class BrewersDbContextFactory : IDesignTimeDbContextFactory<BrewersDbContext>
{
    public BrewersDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__BrewersDb")
            ?? "Server=localhost,1433;Database=BrewersDb;User Id=sa;Password=design-time-only;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<BrewersDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        return new BrewersDbContext(options);
    }
}

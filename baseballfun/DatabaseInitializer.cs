using baseballfun.Data;
using Microsoft.EntityFrameworkCore;

namespace baseballfun;

public static class DatabaseInitializer
{
    private const int MaxAttempts = 10;

    /// <summary>
    /// Applies pending migrations and seeds the mock data. Retries with backoff so the API survives
    /// starting before SQL Server is ready. Does nothing when running on in-memory storage or when
    /// Database:InitializeOnStartup is false.
    /// </summary>
    public static async Task InitializeDatabaseAsync(this WebApplication app, CancellationToken ct = default)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInitializer");

        if (!app.Configuration.GetValue("Database:InitializeOnStartup", true))
        {
            logger.LogInformation("Database initialization is disabled by configuration");
            return;
        }

        using (var probe = app.Services.CreateScope())
        {
            if (probe.ServiceProvider.GetService<BrewersDbContext>() is null)
            {
                return;
            }
        }

        for (var attempt = 1; ; attempt++)
        {
            try
            {
                // A fresh scope per attempt avoids reusing a context left in a bad state by a failed attempt.
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<BrewersDbContext>();

                await db.Database.MigrateAsync(ct);
                logger.LogInformation("Database migrations applied");
                await BrewersSeeder.SeedAsync(db, logger, ct);
                return;
            }
            catch (Exception ex) when (attempt < MaxAttempts && !ct.IsCancellationRequested)
            {
                var delay = TimeSpan.FromSeconds(Math.Min(Math.Pow(2, attempt), 15));
                logger.LogWarning(ex,
                    "Database initialization attempt {Attempt} of {MaxAttempts} failed; retrying in {DelaySeconds}s",
                    attempt, MaxAttempts, delay.TotalSeconds);
                await Task.Delay(delay, ct);
            }
        }
    }
}

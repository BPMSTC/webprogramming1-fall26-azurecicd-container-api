using System.Text.Json.Serialization;
using baseballfun.Data;
using baseballfun.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

// SQL Server is the default. Set Storage:Provider=InMemory (env var Storage__Provider) to opt in to throwaway storage.
if (string.Equals(builder.Configuration["Storage:Provider"], "InMemory", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
    builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("BrewersDb")
        ?? throw new InvalidOperationException(
            "ConnectionStrings:BrewersDb is not configured. Set the ConnectionStrings__BrewersDb environment variable, " +
            "or set Storage__Provider=InMemory.");

    builder.Services.AddDbContext<BrewersDbContext>(options => options.UseSqlServer(
        connectionString,
        sql => sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));
    builder.Services.AddScoped<IPlayerRepository, SqlPlayerRepository>();
    builder.Services.AddScoped<IGameRepository, SqlGameRepository>();
}

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using LoLTournaments.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Di Setup
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("PostgresDbConnection"),
            o =>
            {
                o.CommandTimeout(360);
                o.EnableRetryOnFailure();
                o.MigrationsHistoryTable("__EFMigrationsHistory", "public");
            });
});
// builder.Services.AddScoped<IDbRepository, DbRepository>();
// Di Setup

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();
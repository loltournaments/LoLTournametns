using Microsoft.EntityFrameworkCore;

namespace LoLTournaments.Infrastructure.Presistence.DbSeed
{

    public class DbSeedService<TContext> : IDbSeedService where TContext : DbContext
    {
        private readonly TContext dbContext;

        public DbSeedService(TContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CleanUp()
        {
            await dbContext.SaveChangesAsync();
        }

        public Task Seed()
        {
            return Task.CompletedTask;
        }

        public async Task Migrate()
        {
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await dbContext.Database.MigrateAsync();
        }
    }

}
namespace LoLTournaments.Infrastructure.Presistence.DbSeed
{
    public interface IDbSeedService
    {
        Task Seed();

        Task Migrate();

        Task CleanUp();
    }
}

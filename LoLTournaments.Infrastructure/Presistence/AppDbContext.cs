using LoLTournaments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoLTournaments.Infrastructure.Presistence
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}


        public DbSet<UserEntity> Users { get; set; }
    }

}
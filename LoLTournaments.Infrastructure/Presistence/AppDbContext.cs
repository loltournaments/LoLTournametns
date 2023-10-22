using LoLTournaments.Domain.Entities;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoLTournaments.Infrastructure.Presistence
{

    public class AppDbContext : IdentityDbContext<UserEntity>
    {
        public AppDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(builder =>
            {
                builder.Property(x => x.Permission).HasConversion(x => x.ToString(), x => x.ToEnum<Permissions>());
            });
            base.OnModelCreating(modelBuilder);
        }
    }

}
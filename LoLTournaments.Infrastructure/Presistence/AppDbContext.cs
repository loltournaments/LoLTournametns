using LoLTournaments.Domain.Entities;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        
        public DbSet<SessionEntity> Sessions { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
    }

}
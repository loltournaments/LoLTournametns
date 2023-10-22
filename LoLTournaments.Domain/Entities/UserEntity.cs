using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace LoLTournaments.Domain.Entities
{
    public class UserEntity : IdentityUser, IEntity
    {
        public bool Tutorial { get; set; } = false;
        public Permissions Permission { get; set; } = Permissions.Participant;
        
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime OnUpdated { get; set; } = DateTime.UtcNow;
    }
}
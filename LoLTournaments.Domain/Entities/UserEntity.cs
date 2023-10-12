using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace LoLTournaments.Domain.Entities
{
    public class UserEntity : IdentityUser, IEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public bool Tutorial { get; set; } = false;
        public Permissions Permission { get; set; } = Permissions.Viewer | Permissions.Participant;
        
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime OnUpdated { get; set; } = DateTime.UtcNow;
    }
}
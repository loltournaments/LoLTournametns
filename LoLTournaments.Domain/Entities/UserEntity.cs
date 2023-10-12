using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public bool Tutorial { get; set; } = false;
        public Permissions Permission { get; set; } = Permissions.Viewer | Permissions.Participant;
    }
}
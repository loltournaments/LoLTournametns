using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Application.Models
{

    public class RuntimeSession : DataBase, IOrderable
    {
        public int Order { get; set; }
        public int Step { get; set; } = -4;
        public SynchronizedCollection<RuntimeStage> Stages { get; set; } = new();
        public SynchronizedCollection<RuntimeMember> Members { get; set; } = new();
        public SynchronizedCollection<RuntimeWinner> Winners { get; set; } = new();
    }

}
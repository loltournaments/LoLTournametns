using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Application.Runtime
{

    public class RuntimeStage : DataBase, IOrderable
    {
        public int Order { get; set; }
        public SynchronizedCollection<RuntimeGroup> Groups { get; set; } = new();
        public SynchronizedCollection<string> Winners { get; set; } = new();
        public StageState State { get; set; }
    }

}
using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Application.Runtime
{

    public class RuntimeGame : DataBase, IOrderable
    {
        public int Tour { get; set; }
        public int Order { get; set; }
        public string WinnerId { get; set; }
        public WinnerReason WinnerReason { get; set; }

        public SessionTimer Timer { get; set; }
        public SynchronizedCollection<string> Members { get; set; } = new();
    }

}
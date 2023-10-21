using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Application.Runtime
{

    public class RuntimeGroup : DataBase, IOrderable
    {
        public string Tag { get; set; }
        public int Order { get; set; }

        public SynchronizedCollection<RuntimeGame> Games { get; set; } = new();
        public SynchronizedCollection<string> Members { get; set; } = new();
    }

}
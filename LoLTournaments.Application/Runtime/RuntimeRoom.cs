using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Models;
using Newtonsoft.Json;

namespace LoLTournaments.Application.Runtime
{

    public class RuntimeRoom : DataBase, IOrderable
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public LobbyState State { get; set; }
        public SessionTimer Timer { get; set; }
        [JsonIgnore] public bool HasChanges { get; set; }
        public SynchronizedCollection<ParamInfo> Info { get; set; } = new();
        public SynchronizedCollection<string> Registered { get; set; } = new();
        public SynchronizedCollection<string> Accepted { get; set; } = new();
    }

}
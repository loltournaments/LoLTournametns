using System.Collections.Generic;
using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Models
{

    public class Game : DataBase, IOrderable
    {
        public int Tour { get; set; }
        public int Order { get; set; }
        public string WinnerId { get; set; }
        public WinnerReason WinnerReason { get; set; }

        public SessionTimer Timer { get; set; }
        public List<string> Members { get; set; } = new();
    }

}
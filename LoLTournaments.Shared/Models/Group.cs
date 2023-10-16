using System.Collections.Generic;
using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Models
{
    public class Group : DataBase, IOrderable
    {
        public string Tag { get; set; }
        public int Order { get; set; }

        public List<Game> Games = new();
        public List<string> Members = new();
    }
}
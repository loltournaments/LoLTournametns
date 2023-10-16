using System.Collections.Generic;
using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Models
{
    public class Stage : DataBase, IOrderable
    {
        public int Order { get; set; }
        public List<Group> Groups = new();
        public List<string> Winners = new();
        public StageState State { get; set; }
    }
}
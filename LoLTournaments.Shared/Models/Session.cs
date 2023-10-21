using System.Collections.Generic;
using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Models
{

    public class Session : DataBase
    {
        public int Step { get; set; } = -4;
        public List<Stage> Stages { get; set; } = new();
        public List<Member> Members { get; set; } = new();
        public List<Winner> Winners { get; set; } = new();
    }

}
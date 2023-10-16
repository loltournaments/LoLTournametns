using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Models
{
    public class ParamInfo : DataBase, IOrderable
    {
        public string Value { get; set; }
        public int Order { get; set; }
    }
}
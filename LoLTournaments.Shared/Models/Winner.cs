using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Models
{
    public class Winner : Member , IOrderable
    {
        public int Order { get; set; }
    }
}
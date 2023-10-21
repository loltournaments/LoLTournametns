using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Application.Runtime
{

    public class RuntimeWinner : RuntimeMember, IOrderable
    {
        public int Order { get; set; }
    }

}
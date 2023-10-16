using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Models
{
    public class Member : DataBase
    {
        public string NickName { get; set; }
        public MemberState State { get; set; }
    }
}
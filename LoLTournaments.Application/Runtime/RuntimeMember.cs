using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Application.Runtime
{

    public class RuntimeMember : DataBase
    {
        public string NickName { get; set; }
        public MemberState State { get; set; }
    }

}
using System;
using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Models
{
    [Serializable]
    public class Account : DataBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Tutorial { get; set; }
        public Permissions Permission { get; set; }
    }
}
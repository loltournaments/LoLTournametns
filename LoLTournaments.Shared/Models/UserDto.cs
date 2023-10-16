using System;
using LoLTournaments.Shared.Abstractions;
using Newtonsoft.Json;

namespace LoLTournaments.Shared.Models
{
    [Serializable]
    public class UserDto : DataBase
    {
        [JsonProperty("username")]
        public string Name { get; set; }
        [JsonProperty("password")]
        public string Pin { get; set; }
        public bool Tutorial { get; set; }
        public Permissions Permission { get; set; }
    }
}
using Newtonsoft.Json;

namespace LoLTournaments.Shared.Models
{
    public class LeagueOfLegendsPlayerInfo
    {
        [JsonProperty("puuid")] 
        public string Id;
        [JsonProperty("icon")] 
        public string IconUrl;
        public string Region;
        public string Name;
        public string Level;
        [JsonProperty("solo-tier")] 
        public string Tier;
    }

}
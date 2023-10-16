using System;
using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Shared.Models
{
    [Serializable]
    public class SharedConfig : ISharedConfig
    {
        public string IconsPath { get; set; }
        public string StatsPath { get; set; }
        public string GroupeTags { get; set; }
        public int MaxTourInGroup { get; set; }
        public int MaxGameInGroup { get; set; }
        public int MaxMembersInGame { get; set; }
        public int MaxMembersInGroup { get; set; }
        public int MinMembersInGroup { get; set; }
        public int MaxGroupHorizontalInStage { get; set; }
        public int MinHalfFinalMembers { get; set; }
        public int MaxHalfFinalMembers  { get; set; }
        public int MinHalfFinalGroups { get; set; }
        public int MinMembersToFillBots { get; set; }
        public string BotName { get; set; }
        public string BotId { get; set; }
        public double LobbyProcessTime { get; set; }
        public double LobbyAcceptsTime { get; set; }
        public double GameAcceptsTime { get; set; }
        public DateTime RemoteTime { get; set; }
        public string RemoteTimeAbbreviation { get; set; }
    }
}
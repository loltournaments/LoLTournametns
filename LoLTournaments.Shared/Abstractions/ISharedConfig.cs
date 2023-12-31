﻿namespace LoLTournaments.Shared.Abstractions
{

    public interface ISharedConfig
    {
        string Region { get; }
        string GroupeTags { get; }
        int MaxTourInGroup { get; }
        int MaxGameInGroup { get; }
        int MaxMembersInGame { get; }
        int MaxMembersInGroup { get; }
        int MinMembersInGroup { get; }
        int MaxGroupHorizontalInStage { get; }
        int MinHalfFinalMembers { get; }
        int MaxHalfFinalMembers { get; }
        int MinHalfFinalGroups { get; }
        int MaxWinCountPlaces { get; }
        int MinMembersToFillBots { get; }
        string BotName { get; }
        string BotId { get; }
        double LobbyProcessTime { get; }
        double LobbyAcceptsTime { get; }
        double GameAcceptsTime { get; }
    }

}
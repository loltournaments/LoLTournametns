using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Domain.Abstractions
{

    public interface IAppSettings : ISharedConfig
    {
        string Version { get; }
        string IconsPath { get; }
        string StatsPath { get; }
        string FakeAccountPath { get; }
        int FakeAccountCount { get; }
        bool IsMaintenanceMode { get; set; }
        string TimeAbbrevation { get; }
    }

}
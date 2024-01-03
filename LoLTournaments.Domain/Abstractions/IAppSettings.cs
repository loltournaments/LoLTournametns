using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Domain.Abstractions
{

    public interface IAppSettings : ISharedConfig
    {
        string Version { get; }
        string IconsPath { get; }
        string StatsPath { get; }
        List<string> FakeUserNames { get; }
        bool IsMaintenanceMode { get; set; }
        string TimeAbbrevation { get; }
    }

}
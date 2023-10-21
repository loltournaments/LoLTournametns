using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Domain.Abstractions
{

    public interface IAppSettings : ISharedConfig
    {
        string Version { get; }
        bool IsMaintenanceMode { get; set; }
        string TimeAbbrevation { get; }
    }

}
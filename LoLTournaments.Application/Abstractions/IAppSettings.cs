using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Application.Abstractions
{

    public interface IAppSettings : ISharedConfig
    {
        string Version { get; }
        bool IsMaintenanceMode { get; set; }
    }

}
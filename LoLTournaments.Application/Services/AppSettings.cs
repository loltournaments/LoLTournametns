using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Application.Services
{

    public class AppSettings : SharedConfig, IAppSettings
    {
        public string Version { get; set; }
        public string IconsPath { get; set; }
        public string StatsPath { get; set; }
        public bool IsMaintenanceMode { get; set; }
        public string TimeAbbrevation { get; set; }
    }

}
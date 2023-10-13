namespace LoLTournaments.Application.Services
{

    public interface IAppSettings
    {
        string Version { get; }
        bool IsMaintenanceMode { get; set; }
    }
    
    public class AppSettings : IAppSettings
    {
        public string Version { get; set; }
        public bool IsMaintenanceMode { get; set; }
    }

}
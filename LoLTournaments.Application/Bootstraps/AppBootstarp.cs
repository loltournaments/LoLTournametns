using LoLTournaments.Application.Runtime;
using LoLTournaments.Application.Services;

namespace LoLTournaments.Application.Bootstraps
{

    public class AppBootstarp : IBootstrapCmd
    {
        private readonly IRuntimeBackupService<RuntimeRoom> roomBackupService;
        private readonly IRuntimeBackupService<RuntimeSession> sessionsBackupService;

        public AppBootstarp(
            IRuntimeBackupService<RuntimeRoom> roomBackupService,
            IRuntimeBackupService<RuntimeSession> sessionsBackupService)
        {
            this.roomBackupService = roomBackupService;
            this.sessionsBackupService = sessionsBackupService;
        }

        public async Task ExecuteAsync()
        {
            await roomBackupService.RestoreAsync();
            await sessionsBackupService.RestoreAsync();
        }
    }

}
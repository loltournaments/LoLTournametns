using LoLTournaments.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoLTournaments.Application.Infrastructure
{

    public class ApplicationInstaller
    {
        public static void Install(IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ILobbyService, LobbyService>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddSingleton<IAppSettings>(appSettings);  
        }
    }

}
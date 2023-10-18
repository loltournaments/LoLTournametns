using LoLTournaments.Application.Abstractions;
using LoLTournaments.Application.Services;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Infrastructure.Presistence;
using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoLTournaments.Application.Infrastructure
{

    public class ApplicationDi
    {
        public static void Install(IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddAutoMapper(typeof(AppMappingProfile));
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ILobbyService, LobbyService>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddSingleton<IAppSettings>(appSettings);  
            services.AddSingleton<ISharedTime, SharedTime>();  
            services.AddSingleton<ISharedConfig>(appSettings);  
            services.AddScoped<IDbRepository, DbRepository>();
        }
    }

}
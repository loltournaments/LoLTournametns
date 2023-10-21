using LoLTournaments.Application.Runtime;
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
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ILobbyService, LobbyService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IAccountInfoService, AccountInfoService>();
            services.AddScoped<IDbRepository, DbRepository<AppDbContext>>();
            services.AddScoped<ISharedTime, SharedTime>();
            
            services.AddSingleton<IAppSettings>(appSettings);
            services.AddSingleton<ISharedConfig>(appSettings);  
            services.AddSingleton<IRuntimeRepository<RuntimeRoom>, RuntimeRepository<RuntimeRoom>>();  
            services.AddSingleton<IRuntimeRepository<RuntimeSession>, RuntimeRepository<RuntimeSession>>();
        }
    }

}
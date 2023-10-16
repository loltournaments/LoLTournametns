using AutoMapper;
using LoLTournaments.Application.Abstractions;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Application.Services
{

    public interface IIdentityService
    {
        Task Login();
        Task Register();
        Task Authenticate();
        Task ResetPassword();
        object GetConfig();
    }
    public class IdentityService : IIdentityService
    {
        private readonly IMapper mapper;
        private readonly IAppSettings appSettings;
        public IdentityService(
            IMapper mapper,
            IAppSettings appSettings)
        {
            this.mapper = mapper;
            this.appSettings = appSettings;
        }

        public Task Login()
        {
            throw new NotImplementedException();
        }

        public Task Register()
        {
            throw new NotImplementedException();
        }

        public Task Authenticate()
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword()
        {
            throw new NotImplementedException();
        }

        public object GetConfig()
        {
            return mapper.Map<SharedConfig>(appSettings);
        }
    }

}
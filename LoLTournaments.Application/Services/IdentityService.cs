using AutoMapper;
using LoLTournaments.Application.Abstractions;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Domain.Entities;
using LoLTournaments.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace LoLTournaments.Application.Services
{

    public interface IIdentityService
    {
        Task Login();
        Task Register(UserDto model);
        Task Authenticate();
        Task ResetPassword();
        object GetConfig();
    }
    public class IdentityService : IIdentityService
    {
        private readonly IMapper mapper;
        private readonly IAppSettings appSettings;
        private readonly IDbRepository dbRepository;
        private readonly UserManager<UserEntity> userManager;

        public IdentityService(
            IMapper mapper,
            IAppSettings appSettings,
            IDbRepository dbRepository,
            UserManager<UserEntity> userManager)
        {
            this.mapper = mapper;
            this.appSettings = appSettings;
            this.dbRepository = dbRepository;
            this.userManager = userManager;
        }

        public Task Login()
        {
            throw new NotImplementedException();
        }

        public Task Register(UserDto model)
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
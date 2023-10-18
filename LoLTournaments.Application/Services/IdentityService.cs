using System.Security.Claims;
using AutoMapper;
using CCG.Berserk.Application.Exceptions;
using LoLTournaments.Application.Abstractions;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Domain.Entities;
using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoLTournaments.Application.Services
{

    public interface IIdentityService
    {
        Task<UserDto> Login(UserDto user);
        Task<UserDto> Register(UserDto model);
        Task<UserDto> ResetPassword(UserDto model);
        object GetConfig();
        DateTime GetCurrentTime();
        string GetTimeAbbrevation();
    }
    public class IdentityService : IIdentityService
    {
        private readonly IMapper mapper;
        private readonly ISharedTime sharedTime;
        private readonly IAppSettings appSettings;
        private readonly IDbRepository dbRepository;
        private readonly UserManager<UserEntity> userManager;
        private readonly SignInManager<UserEntity> signInManager;

        public IdentityService(
            IMapper mapper,
            ISharedTime sharedTime,
            IAppSettings appSettings,
            IDbRepository dbRepository,
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager)
        {
            this.mapper = mapper;
            this.sharedTime = sharedTime;
            this.appSettings = appSettings;
            this.dbRepository = dbRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<UserDto> Login(UserDto model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user == null)
                return await Register(model);

            if (!await userManager.CheckPasswordAsync(user, model.Password))
                throw new ValidationException($"Invalid Credentials");

            if (appSettings.IsMaintenanceMode && !user.Permission.HasAllFlags(Permissions.Manager))
                throw new ValidationException($"We are updating app servers to provide you with the best experience possible.");

            await signInManager.SignInAsync(user, new AuthenticationProperties {IsPersistent = true});
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> Register(UserDto model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user != null)
                return mapper.Map<UserDto>(user);

            user = mapper.Map<UserEntity>(model);
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Errors.Any())
                throw new ValidationException($"Registration error with messages: {string.Join(',', result.Errors.Select(x => x.Description))}");
            
            await userManager.AddClaimsAsync(user, new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, model.UserName),
                new(ClaimTypes.Role, "Player")
            });

            return mapper.Map<UserDto>(user);
        }

        public Task Authenticate()
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> ResetPassword(UserDto model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user == null)
                throw new ValidationException($"User {model.UserName} doesn't exist");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Errors.Any())
                throw new ValidationException($"Reset password error with messages: {string.Join('\n', result.Errors.Select(x => x.Description))}");
            
            return mapper.Map<UserDto>(user);
        }

        public object GetConfig()
        {
            return mapper.Map<SharedConfig>(appSettings);
        }

        public DateTime GetCurrentTime()
        {
            return sharedTime.Current;
        }

        public string GetTimeAbbrevation()
        {
            return appSettings.TimeAbbrevation;
        }
    }

}
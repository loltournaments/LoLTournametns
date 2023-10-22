using System.Security.Claims;
using AutoMapper;
using LoLTournaments.Application.Exceptions;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Domain.Entities;
using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace LoLTournaments.Application.Services
{

    public interface IIdentityService
    {
        Task<Account> Login(Account user);
        Task<Account> Register(Account model);
        Task<Account> ResetPassword(Account model);
        object GetConfig();
        ApiTime GetApiTime();
    }

    public class IdentityService : IIdentityService
    {
        private readonly IMapper mapper;
        private readonly ISharedTime sharedTime;
        private readonly IAppSettings appSettings;
        private readonly UserManager<UserEntity> userManager;
        private readonly SignInManager<UserEntity> signInManager;

        public IdentityService(
            IMapper mapper,
            ISharedTime sharedTime,
            IAppSettings appSettings,
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager)
        {
            this.mapper = mapper;
            this.sharedTime = sharedTime;
            this.appSettings = appSettings;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<Account> Login(Account model)
        {
            ValidateVersion(model.Version);
            var user = await userManager.FindByNameAsync(model.UserName);
            
            if (appSettings.IsMaintenanceMode && !user.Permission.HasAllFlags(Permissions.Manager))
                throw new ForbiddenException(
                    $"We are updating app servers to provide you with the best experience possible.");
            
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                throw new ValidationException($"Invalid Credentials");

            await signInManager.SignInAsync(user, new AuthenticationProperties {IsPersistent = true});
            return mapper.Map<Account>(user);
        }

        public async Task<Account> Register(Account model)
        {
            ValidateVersion(model.Version);
            model.Permission = Permissions.Participant;
            var user = mapper.Map<UserEntity>(model);
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Errors.Any())
                throw new ServerException($"Registration failed : {string.Join(',', result.Errors.Select(x => x.Description))}");

            await userManager.AddClaimsAsync(user, new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserName),
                new(ClaimTypes.Role, "Player")
            });

            return mapper.Map<Account>(user);
        }

        public async Task<Account> ResetPassword(Account model)
        {
            ValidateVersion(model.Version);
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user == null)
                throw new ValidationException($"User {model.UserName} doesn't exist");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Errors.Any())
                throw new ValidationException(
                    $"Reset password error with messages: {string.Join('\n', result.Errors.Select(x => x.Description))}");

            return mapper.Map<Account>(user);
        }

        public object GetConfig()
        {
            return mapper.Map<SharedConfig>(appSettings);
        }

        public ApiTime GetApiTime()
        {
            return new ApiTime
            {
                Date = sharedTime.Current,
                Abbrevation = appSettings.TimeAbbrevation
            };
        }

        private void ValidateVersion(string clientVersion)
        {
            if (clientVersion.ConvertVersion() >= appSettings.Version.ConvertVersion())
                return;

            throw new ValidationException($"Install the latest version : {appSettings.Version}");
        }
    }

}
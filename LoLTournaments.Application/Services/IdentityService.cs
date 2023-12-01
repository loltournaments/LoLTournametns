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
using Microsoft.EntityFrameworkCore;

namespace LoLTournaments.Application.Services
{

    public interface IIdentityService
    {
        Task<dynamic> Login(Account user);
        Task<dynamic> Register(Account model);
        Task<dynamic> ResetPassword(Account model);
        Task<dynamic> GetAccounts();
        Task SetAccountTutorial(Account model);
        dynamic GetConfig();
        dynamic GetApiTime();
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

        public async Task<dynamic> Login(Account model)
        {
            ValidateVersion(model.Version);
            var user = await userManager.FindByNameAsync(model.UserName);
            
            if (appSettings.IsMaintenanceMode && !user.Permission.HasAllFlags(Permissions.Manager))
                throw new ForbiddenException($"We are updating app server to provide you with the best experience possible.");
            
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
                throw new UnauthorizedHttpException($"Invalid Credentials");

            await signInManager.SignInAsync(user, new AuthenticationProperties {IsPersistent = true});
            return mapper.Map<Account>(user);
        }

        public async Task<dynamic> Register(Account model)
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

        public async Task<dynamic> ResetPassword(Account model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user == null)
                throw new NotFoundException($"User {model.UserName} doesn't exist");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Errors.Any())
                throw new ValidationException(
                    $"Reset password error with messages: {string.Join('\n', result.Errors.Select(x => x.Description))}");

            return mapper.Map<Account>(user);
        }

        public async Task<dynamic> GetAccounts()
        {
            return mapper.Map<Account[]>(await dbRepository.Get<UserEntity>().ToArrayAsync());
        }

        public async Task SetAccountTutorial(Account model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            user.Tutorial = model.Tutorial;
            await dbRepository.SaveChangesAsync();
        }

        public dynamic GetConfig()
        {
            return mapper.Map<SharedConfig>(appSettings);
        }

        public dynamic GetApiTime()
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
using System.Security.Claims;
using AutoMapper;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Domain.Entities;
using LoLTournaments.Shared.Common;
using LoLTournaments.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LoLTournaments.Application.Services
{

    public class FakeName
    {
        public string Name { get; set; }
    }

    public interface IFakeAccountService
    {
        Task<Account[]> GenerateAsync(int? count = null);
        Task<Account> AddBotAccount();
    }

    public class FakeAccountService : IFakeAccountService
    {
        private const string DefaultPassword = "1q2w";
        private const string DefaultRole = "FakePlayer";
        private const Permissions DefaultPermissions = Permissions.Test;
        
        private readonly IMapper mapper;
        private readonly IAppSettings appSettings;
        private readonly IDbRepository dbRepository;
        private readonly UserManager<UserEntity> userManager;

        public FakeAccountService(
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

        public async Task<Account[]> GenerateAsync(int? count = null)
        {
            var fakeAccountCount = count ?? appSettings.FakeAccountCount;
            var fakeAccount = await dbRepository.Get<UserEntity>()
                .Where(x => x.Permission == DefaultPermissions)
                .ToListAsync();
            
            var existNames = fakeAccount
                .Select(x => x.UserName)
                .ToList();

            if (existNames.Count >= appSettings.FakeAccountCount)
                return mapper.Map<Account[]>(fakeAccount);

            var retryCount = 0;
            var newUserNames = new List<string>();

            while (existNames.Count + newUserNames.Count < fakeAccountCount && retryCount < 3)
            {
                try
                {
                    using var client = new HttpClient();
                    client.Timeout = Timeout.InfiniteTimeSpan;
                    using var request = new HttpRequestMessage(HttpMethod.Get, appSettings.FakeAccountPath);
                    using var response = await client.SendAsync(request);
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var fakeName = JsonConvert.DeserializeObject<FakeName>(jsonData);

                    if (newUserNames.Contains(fakeName!.Name))
                        continue;

                    newUserNames.Add(fakeName.Name);
                    retryCount = 0;
                }
                catch (Exception e)
                {
                    DefaultSharedLogger.Error(e);
                    retryCount++;
                }
            }

            var fakeAccounts = newUserNames.Distinct().Select(name => new Account
            {
                UserName = name,
                Permission = DefaultPermissions,
                Tutorial = true
            }).ToList();

            var fakeUsers = mapper.Map<UserEntity[]>(fakeAccounts);
            foreach (var fakeUser in fakeUsers)
            {
                var result = await userManager.CreateAsync(fakeUser, DefaultPassword);
                if (result.Errors.Any())
                    DefaultSharedLogger.Error(
                        $"Fake account registration failed : {string.Join(',', result.Errors.Select(x => x.Description))}");

                await userManager.AddClaimsAsync(fakeUser, new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, fakeUser.UserName),
                    new(ClaimTypes.Role, DefaultRole)
                });
            }

            return fakeAccounts.ToArray();
        }

        public async Task<Account> AddBotAccount()
        {
            var botUser = await userManager.FindByNameAsync(appSettings.BotName);
            if (botUser != null)
                return mapper.Map<Account>(botUser);

            var botAccount = new Account
            {
                Id = appSettings.BotId,
                UserName = appSettings.BotName,
                Permission = Permissions.Participant
            };

            botUser = mapper.Map<UserEntity>(botAccount);
            var result = await userManager.CreateAsync(botUser, DefaultPassword);
            if (result.Errors.Any())
                DefaultSharedLogger.Error(
                    $"Add bot user failed : {string.Join(',', result.Errors.Select(x => x.Description))}");

            await userManager.AddClaimsAsync(botUser, new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, botUser.UserName),
                new(ClaimTypes.Role, appSettings.BotName)
            });

            return botAccount;
        }
    }

}
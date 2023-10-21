using AutoMapper;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Domain.Entities;
using LoLTournaments.Shared.Common;
using LoLTournaments.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace LoLTournaments.Application.Services
{
    public interface IAccountInfoService
    {
        Task<AccountInfo> GetInfo(string userName);
    }

    public class AccountInfoService : IAccountInfoService
    {
        private readonly IMapper mapper;
        private readonly IAppSettings appSettings;
        private readonly UserManager<UserEntity> userManager;

        public AccountInfoService(
            IMapper mapper,
            IAppSettings appSettings,
            UserManager<UserEntity> userManager)
        {
            this.mapper = mapper;
            this.appSettings = appSettings;
            this.userManager = userManager;
        }
        
        public async Task<AccountInfo> GetInfo(string userName)
        {
            var url = appSettings.StatsPath
                .Replace("[region]", appSettings.Region)
                .Replace("[name]", userName)
                .Replace(" ", "%20");

            try
            {
                using var client = new HttpClient();
                using var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                var jsonData = await response.Content.ReadAsStringAsync();
                var playerInfo = JsonConvert.DeserializeObject<LeagueOfLegendsPlayerInfo>(jsonData);
                var accountInfo = mapper.Map<AccountInfo>(playerInfo);
                accountInfo.IconUrl = appSettings.IconsPath.Replace("[name]", accountInfo.IconUrl).Replace(" ", "%20");

                if (accountInfo.Available)
                    accountInfo.Known = (await userManager.FindByNameAsync(userName)) != null;
                
                return accountInfo;
            }
            catch (Exception e)
            {
                DefaultSharedLogger.Error(e);
                throw;
            }
        }
    }

}
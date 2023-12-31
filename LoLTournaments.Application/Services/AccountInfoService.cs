﻿using AutoMapper;
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

    public class LeagueOfLegendsPlayerInfo
    {
        [JsonProperty("puuid")] 
        public string Id;
        [JsonProperty("icon")] 
        public string IconUrl;
        public string Region;
        public string Name;
        public string Level;
        [JsonProperty("solo-tier")] 
        public string Tier;
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
            try
            {
                var url = appSettings.StatsPath
                    .Replace("[region]", appSettings.Region)
                    .Replace("[name]", userName.Replace("#","-"))
                    .Replace(" ", "%20");
                
                using var client = new HttpClient();
                using var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                var jsonData = await response.Content.ReadAsStringAsync();
                var playerInfo = JsonConvert.DeserializeObject<LeagueOfLegendsPlayerInfo>(jsonData);
                var accountInfo = mapper.Map<AccountInfo>(playerInfo);

                accountInfo.IconUrl = string.IsNullOrEmpty(accountInfo.IconUrl) ? "default" : appSettings.IconsPath.Replace("[name]", accountInfo.IconUrl).Replace(" ", "%20");
                accountInfo.Known = (await userManager.FindByNameAsync(userName)) != null;
                accountInfo.Name ??= userName;
                
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
﻿using LoLTournaments.Application.Abstractions;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Application.Services
{

    public class AppSettings : SharedConfig, IAppSettings
    {
        public string Version { get; set; }
        public bool IsMaintenanceMode { get; set; }
    }

}
﻿using System;
using System.Collections.Generic;
using LoLTournaments.Shared.Abstractions;
using Newtonsoft.Json;

namespace LoLTournaments.Shared.Models
{
    public class Room : DataBase, IOrderable
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public LobbyState State { get; set; }
        public SessionTimer Timer { get; set; }
        
        [JsonIgnore] public bool HasChanges { get; set; }

        public List<ParamInfo> Info  { get; set; } = new();
        public List<string> Registered  { get; set; } = new();
        public List<string> Accepted  { get; set; } = new();
    }
}
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoLTournaments.Shared.Models
{
    [Flags,JsonConverter(typeof(StringEnumConverter))]
    public enum LobbyState
    {
        Draft = 0,       
        Registration = 8,
        Accepts = 16,
        Processing = 32,  
        CanEnter = 64,    
        Closed = 128,      
    }
}
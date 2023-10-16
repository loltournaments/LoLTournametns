using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoLTournaments.Shared.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MemberState
    {
        Admitted = 0,       
        NotEligible = 1   
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoLTournaments.Shared.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StageState
    {
        Closed = 0,
        Active = 1,
    }
}
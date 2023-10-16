using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoLTournaments.Shared.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WinnerReason
    {
        None,
        Regular,
        TeachWin,
        TechLoose
    }

}
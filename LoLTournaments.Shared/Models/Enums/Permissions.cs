using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoLTournaments.Shared.Models
{

    [Serializable, Flags, JsonConverter(typeof(StringEnumConverter))]
    public enum Permissions
    {
        Developer = -1,
        Viewer = 8,
        Participant = 16 | Viewer,
        Manager = 32 | Participant | Viewer,
        Test = 64 | Participant | Viewer,
    }

}
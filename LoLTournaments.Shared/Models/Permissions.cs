using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LoLTournaments.Shared.Models
{

    [Serializable, Flags, JsonConverter(typeof(StringEnumConverter))]
    public enum Permissions
    {
        Developer = -1, // Полный доступ
        Viewer = 8, // простой доступ
        Participant = 16, // участник
        Manager = 32 | Participant | Viewer, // управление турнирами
        Test = 64 | Participant | Viewer, // управление турнирами
    }

}
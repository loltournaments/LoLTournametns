using Newtonsoft.Json;

namespace LoLTournaments.Shared.Models
{

    public class ReceiveSessionData : RequestSessionData
    {
        public ApiData Data { get; }

        public ReceiveSessionData(object value)
        {
            Data = new ApiData(value);
        }

        [JsonConstructor]
        public ReceiveSessionData(ApiData data)
        {
            Data = data;
        }
    }

}
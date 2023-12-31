using Newtonsoft.Json;

namespace LoLTournaments.Shared.Models
{

    public class ReceiveGameData : RequestGameData
    {
        public ApiData Data { get; }

        public ReceiveGameData(object value)
        {
            Data = new ApiData(value);
        }

        [JsonConstructor]
        public ReceiveGameData(ApiData data)
        {
            Data = data;
        }
    }

}
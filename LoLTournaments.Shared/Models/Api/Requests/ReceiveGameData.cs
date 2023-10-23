using Newtonsoft.Json;

namespace LoLTournaments.Shared.Models
{

    public class ReceiveGameData : RequestGame
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
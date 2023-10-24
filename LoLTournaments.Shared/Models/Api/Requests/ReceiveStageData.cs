using Newtonsoft.Json;

namespace LoLTournaments.Shared.Models
{

    public class ReceiveStageData : RequestStageData
    {
        public ApiData Data { get; }

        public ReceiveStageData(object value)
        {
            Data = new ApiData(value);
        }

        [JsonConstructor]
        public ReceiveStageData(ApiData data)
        {
            Data = data;
        }
    }

}
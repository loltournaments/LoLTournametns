using Newtonsoft.Json;

namespace LoLTournaments.Shared.Models
{

    public class ReceiveGroupData : RequestGroup
    {
        public ApiData Data { get; }

        public ReceiveGroupData(object value)
        {
            Data = new ApiData(value);
        }

        [JsonConstructor]
        public ReceiveGroupData(ApiData data)
        {
            Data = data;
        }
    }

}
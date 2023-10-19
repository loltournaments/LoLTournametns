using System;
using Newtonsoft.Json;

namespace LoLTournaments.Shared.Models
{

    public class ReceiveSessionData : RequestSessionData
    {
        [JsonRequired] private readonly string value;
        [JsonRequired] private readonly Type valueType;

        public ReceiveSessionData(object value)
        {
            this.value = value is string stringValue ? stringValue : JsonConvert.SerializeObject(value);
            valueType = value.GetType();
        }

        [JsonConstructor]
        public ReceiveSessionData(string value, Type valueType)
        {
            this.value = value;
            this.valueType = valueType;
        }
        
        public object GetValue()
        {
            return valueType == typeof(string) ? value : JsonConvert.DeserializeObject(value, valueType);
        }

        public T GetValue<T>()
        {
            if (valueType != typeof(string)) 
                return (T)GetValue();
            
            if (value is T tValue)
                return tValue;

            throw new InvalidCastException($"Can't cast [{valueType.Name}] to [{typeof(T).Name}]");
        }

        public T GetValue<T>(T _) => GetValue<T>();
    }

}
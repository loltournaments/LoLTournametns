using System;
using LoLTournaments.Shared.Common;
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
        
        public object GetValue(Type customType = null)
        {
            customType ??= valueType;
            return customType == typeof(string) ? value : JsonConvert.DeserializeObject(value, customType);
        }

        public T GetValue<T>()
        {
            if (valueType != typeof(string)) 
                return JsonConvert.DeserializeObject<T>(value);
            
            if (value is T tValue)
                return tValue;

            throw new InvalidCastException($"Can't cast [{valueType.Name}] to [{typeof(T).Name}]");
        }

        public bool TryGetValue<T>(out T result, bool suppreseThrow = true)
        {
            if (suppreseThrow)
            {
                try
                {
                    result = GetValue<T>();
                    return result != null;
                }
                catch (Exception e)
                {
                    DefaultSharedLogger.Error(e);
                    result = default;
                    return false;
                }
            }
            
            result = GetValue<T>();
            return result != null;
        }

        public T GetValue<T>(T _) => GetValue<T>();
    }

}
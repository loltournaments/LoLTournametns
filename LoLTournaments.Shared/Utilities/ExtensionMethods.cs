using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Shared.Utilities
{

    public static class ExtensionMethods
    {
        public static bool HasAnyFlags(this Enum value, Enum flags)
        {
            return value != null
                   && flags != null
                   && (Convert.ToInt32(value) & Convert.ToInt32(flags)) != 0;
        }

        public static bool HasAllFlags(this Enum value, Enum flags)
        {
            return value != null
                   && flags != null
                   && (Convert.ToInt32(value) & Convert.ToInt32(flags)) == Convert.ToInt32(flags);
        }
        
        public static Result<object> GetData(this object source, string propertyName)
        {
            if (source == null)
                return Result.Failure($"Target object not found for [{propertyName}].");
            
            var property = source.GetType().GetProperties()
                .FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));

            if (property == null)
                return Result.Failure($"Requested data : [{propertyName}] not found.");
            
            var obj = new ExpandoObject();
            if (!obj.TryAdd(propertyName, property.GetValue(source)))
                return Result.Failure($"Requested data : [{propertyName}] can't add to {nameof(ExpandoObject)}");
            
            return obj;
        }
        
        /// <summary>
        /// Compare the value to TEnum type
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct
        {
            Enum.TryParse(typeof(TEnum), value, out var result);
            if (result is null)
            {
                return default;
            }
            return (TEnum) result;
        }

        /// <summary>
        /// Compare the value to TEnum type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static TEnum[] ToEnums<TEnum>(this string value, string separator = ",") where TEnum : struct
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<TEnum>();
			
            return value.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.ToEnum<TEnum>())
                .ToArray();
        }
    }

}
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace LoLTournaments.Shared.Utilities
{

    public static class StringExtensions
    {
        public static int ConvertVersion(this string value)
        {
            return !string.IsNullOrEmpty(value)
                ? Convert.ToInt32(string.Join("", value.Where(char.IsDigit)))
                : int.MinValue;
        }
        /// <summary>
        /// Will format any public fields and their values into a string.
        /// </summary>
        public static string ReflectionFormat(this object source) 
        {
            if (source == null)
                return "Object is null";
            
            try
            {
                var type = source.GetType();
                
                if (source is IEnumerable enumerable)
                {
                    var sb = new StringBuilder();
                    foreach (var obj in enumerable)
                        sb.AppendLine(obj.ReflectionFormat());

                    return sb.ToString();
                }
                
                return "\n" + string.Join("\n", type.GetFields().Select(x => $"[{x.Name} : {x.GetValue(source)}]")) + 
                       "\n" + string.Join("\n", type.GetProperties().Select(x => $"[{x.Name} : {x.GetValue(source)}]"));
            }
            catch (Exception)
            {
                return source.ToString();
            }
        }
    }

}
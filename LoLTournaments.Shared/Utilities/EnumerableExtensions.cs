using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LoLTournaments.Shared.Common;

namespace LoLTournaments.Shared.Utilities
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, string sid = null)
        {
            sid ??= Guid.NewGuid().ToString();
            var random = new Random(sid.GetHashCode());
            return enumerable.OrderBy(_ => random.Next());
        }

        public static List<T> ToListFix<T>(this IEnumerable<T> enumerable)
        {
            return enumerable?.ToList() ?? new List<T>();
        }

        public static List<T2> ToListValues<T1,T2>(this IDictionary<T1,T2> dictionary)
        {
            return (dictionary?.Values).ToListFix();
        }

        public static List<T1> ToListKeys<T1,T2>(this IDictionary<T1,T2> dictionary)
        {
            return (dictionary?.Keys).ToListFix();
        }

        public static bool IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicat, out int index)
        {
            index = 0;
            if (enumerable == null || predicat == null)
                return false;

            foreach (var item in enumerable)
            {
                if (predicat.Invoke(item))
                    return true;

                index++;
            }

            return false;
        }

        public static int IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicat)
        {
            if (enumerable == null || predicat == null)
                return -1;
            
            var index = 0;
            foreach (var item in enumerable)
            {
                if (predicat.Invoke(item))
                    return index;

                index++;
            }

            return -1;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static bool InRange<T>(this IEnumerable<T> source, int index)
        {
            return index >= 0 && index < (source?.Count() ?? 0);
        }

        public static bool TryGet<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T result)
        {
            result = default;
            if (predicate == null || source == null)
                return false;

            foreach (var value in source)
            {
                if (!predicate.Invoke(value))
                    continue;

                result = value;
                return true;
            }

            return false;
        }

        public static bool First<T>(this IEnumerable<object> source, out T result)
        {
            result = default;
            if (source == null)
                return false;

            result = source.OfType<T>().FirstOrDefault();

            return result != null;
        }

        public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> result)
        {
            if (enumerable == null || result == null)
                return;

            foreach (var item in enumerable)
                result(item);
        }

        public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T, int> result)
        {
            if (enumerable == null || result == null)
                return;

            var index = 0;
            foreach (var item in enumerable)
                result(item, index++);
        }

        /// <summary>
        /// Replaces an element in the list or add if it new.
        /// For proper replacement you need to implement Equal methods to compare each other.
        /// </summary>
        /// <returns>if replace was successful</returns>
        public static bool Replace(this IList source, object item)
        {
            if (source == null)
            {
                DefaultSharedLogger.Error($"Source IList does not exist");
                return false;
            }
            
            if (item == null)
            {
                DefaultSharedLogger.Error("Item does not exist");
                return false;
            }
            
            var index = source.IndexOf(item);
            var hasItem = index >= 0;
            index = hasItem ? index : Math.Max(0, source.Count - 1);
            if (hasItem)
                source.RemoveAt(index);
            source.Insert(index, item);
            return true;
        }
        
        /// <summary>
        /// Replaces elements in the list or add if it new.
        /// For proper replacement you need to implement Equal methods to compare each other.
        /// </summary>
        /// <returns>if replace was successful</returns>
        public static bool Replace<T>(this IList source, IEnumerable<T> items)
        {
            return items != null && source != null && items.All(item => Replace(source, item));
        }
        
        /// <summary>
        /// Move element in the list to target index.
        /// For proper move you need to implement Equal methods to compare each other.
        /// </summary>
        public static void Move(this IList source, object item, int toIndex = 0)
        {
            if (source == null)
            {
                DefaultSharedLogger.Error($"Source IList does not exist");
                return;
            }
            
            if (item == null)
            {
                DefaultSharedLogger.Error("Item does not exist");
                return;
            }

            if (toIndex < 0 || toIndex >= source.Count)
            {
                DefaultSharedLogger.Error($"Can't move item to index which out of range: {toIndex}, closer: {Math.Clamp(toIndex, 0, source.Count - 1)}");
                return;
            }
            
            var index = source.IndexOf(item);
            if (index < 0)
            {
                DefaultSharedLogger.Error("Item doesn't exist in collection.");
                return;
            }
            
            source.RemoveAt(index);
            source.Insert(Math.Clamp(toIndex, 0, source.Count - 1), item);
        }
    }
}
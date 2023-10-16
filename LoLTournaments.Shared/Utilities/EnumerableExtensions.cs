using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
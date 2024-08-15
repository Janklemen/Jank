using System;
using System.Collections.Generic;
using System.Linq;

namespace Jank.Utilities
{
    public static class UtDictionary
    {
        public static TValue QueryKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, Predicate<TKey> predicate)
        {
            foreach (TKey key in dict.Keys)
            {
                if (predicate(key))
                    return dict[key];
            }

            return default;
        }
        
        public static void ForEachKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey[] keys, Action<TValue> action)
        {
            foreach (TKey key in keys)
                action(dict[key]);
        }

        public static TValue[] GetValues<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<TKey> keys)
        {
            IEnumerable<TKey> enumerable = keys as TKey[] ?? keys.ToArray();
            TValue[] values = new TValue[enumerable.Count()];
            int index = 0;

            foreach (TKey key in enumerable)
                values[index++] = dict[key];

            return values;
        }
        
        public static TValue[] GetValuesFromDictionary<TKey, TValue>(this IEnumerable<TKey> keys, Dictionary<TKey, TValue> dict)
        {
            IEnumerable<TKey> enumerable = keys as TKey[] ?? keys.ToArray();
            TValue[] values = new TValue[enumerable.Count()];
            int index = 0;

            foreach (TKey key in enumerable)
                values[index++] = dict[key];

            return values;
        }
        
        public static (TKey, TValue)[] GetKeyValues<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey[] keys)
        {
            (TKey, TValue)[] values = new (TKey, TValue)[keys.Length];
            int index = 0;

            foreach ((TKey key, TValue value) in dict)
            {
                if(keys.Contains(key))
                    values[index++] = (key, value);
            }

            return values;
        }

        public static ICollection<KeyValuePair<TConvertedKey, TConvertedValue>> Convert<TOriginalKey, TOriginalValue,
            TConvertedKey, TConvertedValue>(this ICollection<KeyValuePair<TOriginalKey, TOriginalValue>> target, Func<KeyValuePair<TOriginalKey, TOriginalValue>, KeyValuePair<TConvertedKey, TConvertedValue>> converter)
        {
            Dictionary<TConvertedKey, TConvertedValue> convert = new();

            foreach (KeyValuePair<TOriginalKey, TOriginalValue> kv in target)
            {
                KeyValuePair<TConvertedKey, TConvertedValue> conversion = converter(kv);
                convert[conversion.Key] = conversion.Value;
            }

            return convert;
        }
    }
}
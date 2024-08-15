using System.Collections.Generic;

namespace Jank.DataStructures.Dictionary
{
    public static class UtDictionary
    {
        /// <summary>
        /// Adds all values from other to target. Overrides repeat keys.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="other"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        public static void Combine<T1, T2>(this Dictionary<T1, T2> target, Dictionary<T1, T2> other)
        {
            foreach (T1 otherKey in other.Keys)
                target[otherKey] = other[otherKey];
        }
    }
}
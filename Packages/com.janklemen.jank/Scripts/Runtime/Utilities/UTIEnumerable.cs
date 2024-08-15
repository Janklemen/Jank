using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Jank.Utilities
{
    public static class UTIEnumerable
    {
        public static void ForEach<T>(this IEnumerable<T> target, Action<T> action)
        {
            foreach (T val in target)
                action(val);
        }
        
        public static void ForEach<T>(this IEnumerable<T> target, Action<T, int> action)
        {
            int index = 0;
            foreach (T val in target)
                action(val, index++);
        }
        
        public static async UniTask ForEachAsync<T>(this IEnumerable<T> target, ActionAsync<T> action)
        {
            foreach (T val in target)
                await action(val);
        }

        public static async UniTask ForEachParallel<T>(this IEnumerable<T> target, ActionAsync<T> action)
        {
            IEnumerable<T> enumerable = target as T[] ?? target.ToArray();
            UniTask[] tasks = new UniTask[enumerable.Count()];
            int index = 0;

            foreach (T val in enumerable)
                tasks[index++] = action(val);
            
            await UniTask.WhenAll(tasks);
        }

        public static IEnumerable<T2> OfType<T1, T2>(this IEnumerable<T1> target)
            where T2: class
        {
            return target
                .Select(t => t as T2)
                .Where(t => t != null);
        }

        public static IEnumerable<T> SelectWhere<T>(this IEnumerable<T> target, Predicate<T> pred)
        {
            IEnumerable<T> enumerable = target as T[] ?? target.ToArray();
            T[] array = new T[enumerable.Count()];
            int index = 0;

            foreach (T element in enumerable)
            {
                if (pred(element))
                    array[index++] = element;
            }

            if (index == 0)
                return Array.Empty<T>();

            return array[..index];
        }
    }
}

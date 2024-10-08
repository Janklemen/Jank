using System;
using System.Collections.Generic;
using System.Linq;
using Jank.DataStructures.Array;

namespace Jank.DotNet.IEnumerable
{
    /// <summary>
    /// Contains utilities for working with enumerables
    /// </summary>
    public static class UTIEnumerable
    {
        /// <summary>
        /// Take multiple IEnumerables and merge them into a single IEnumerable
        /// </summary>
        /// <param name="enumerables">Enumerable of Enumerables</param>
        /// <typeparam name="T">Type of merging enumerable</typeparam>
        /// <returns>single enumerable of type T</returns>  	
        public static IEnumerable<T> Combine<T>(this IEnumerable<IEnumerable<T>> enumerables)
        {
            List<T> obs = new List<T>();

            foreach (IEnumerable<T> en in enumerables)
            {
                foreach (T ob in en)
                {
                    obs.Add(ob);
                }
            }

            return obs;
        }

        /// <summary>
        /// Take multiple IEnumerables and merge them into a single IEnumerable
        /// </summary>
        /// <param name="enumerables">enumerables</param>
        /// <typeparam name="T">Type of merging enumerable</typeparam>
        /// <returns>single enumerable of type T</returns>  	
        public static IEnumerable<T> Combine<T>(params IEnumerable<T>[] enumerables)
        {
            return Combine((IEnumerable<IEnumerable<T>>)enumerables);
        }

        /// <summary>
        /// Take multiple IEnumerables and merge them into a single IEnumerable with unique values
        /// </summary>
        /// <param name="enumerables">Enumerable of Enumerables</param>
        /// <typeparam name="T">Type of merging enumerable</typeparam>
        /// <returns>single enumerable of type T</returns>  	
        public static IEnumerable<T> UniqueMerge<T>(this IEnumerable<IEnumerable<T>> enumerables)
        {

            List<T> obs = new List<T>();

            foreach (IEnumerable<T> en in enumerables)
            {
                foreach (T ob in en)
                {
                    if (!obs.Contains(ob))
                    {
                        obs.Add(ob);
                    }
                }
            }

            return obs;
        }

        /// <summary>
        /// Take multiple IEnumerables and merge them into a single IEnumerable with unique values
        /// </summary>
        /// <param name="enumerables">enumerables</param>
        /// <typeparam name="T">Type of merging enumerable</typeparam>
        /// <returns>single enumerable of type T</returns> 
        public static IEnumerable<T> UniqueMerge<T>(params IEnumerable<T>[] enumerables)
        {
            return UniqueMerge((IEnumerable<IEnumerable<T>>)enumerables);
        }

        /// <summary>
        /// Returns target minus any shared items with test. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The enumerable to return wihtou tests elements</param>
        /// <param name="test">The enumerable whose elements will be removed from target in the reture list</param>
        /// <returns>Enumerable target without shared items in test. If target contains multiple elements, and that element exists in test once, all will be removed</returns>
        public static IEnumerable<T> Difference<T>(this IEnumerable<T> target, IEnumerable<T> test)
        {

            List<T> list = new List<T>();

            IEnumerable<T> enumerable = test as T[] ?? test.ToArray();
            foreach (T item in target)
            {
                if (!enumerable.Contains(item))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// Returns target minus any shared items with test. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The enumerable to return wihtou tests elements</param>
        /// <param name="test">The enumerable whose elements will be removed from target in the reture list</param>
        /// <param name="comparison">Condition for a match</param>
        /// <returns>Enumerable target without shared items in test. If target contains multiple elements, and that element exists in test once, all will be removed</returns>
        public static IEnumerable<T> Difference<T>(this IEnumerable<T> target, IEnumerable<T> test, IEqualityComparer<T> comparison)
        {
            List<T> list = new List<T>();

            IEnumerable<T> enumerable = test as T[] ?? test.ToArray();
            foreach (T item in target)
            {
                if (!enumerable.Contains(item, comparison))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// Check if IEnumerable contains an element that returns true using query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target enumerable</param>
        /// <param name="query">Conditation for positive contains result</param>
        /// <returns></returns>
        public static bool QueryContains<T>(this IEnumerable<T> target, Predicate<T> query)
        {
            foreach (T element in target)
            {
                if (query(element))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Count number of times function returns true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target enumerable</param>
        /// <param name="query">Conditation for positive contains result</param>
        /// <returns></returns>
        public static int QueryCount<T>(this IEnumerable<T> target, Predicate<T> query)
        {
            int counter = 0;

            foreach (T element in target)
            {
                if (query(element))
                {
                    counter++;
                }
            }

            return counter;
        }

        /// <summary>
        /// Get the position in IEnumerable of first true found in query. -1 if never found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target enumerable</param>
        /// <param name="query">Conditation for positive contains result</param>
        /// <returns></returns>
        public static int QueryIndexOf<T>(this IEnumerable<T> target, Predicate<T> query)
        {
            if (target == null)
                return -1;
            
            int index = -1;
            int counter = 0;

            foreach (T element in target)
            {
                if (query(element))
                {
                    index = counter;
                    break;
                }

                counter++;
            }

            return index;
        }

        /// <summary>
        /// Return the first value that meets a predicate or false if can't find it.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="query"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetFirst<T>(this IEnumerable<T> target, Predicate<T> query, out T result)
        {
            if (target != null)
            {
                foreach (T element in target)
                {
                    if (query(element))
                    {
                        result = element;
                        return true;
                    }
                }
            }
            
            result = default;
            return false;
        }

        /// <summary>
        /// Sometimes for some reason you can convert enumerables to types that the 
        /// enumerables type is a child of. For example you can't cast int32[] to
        /// IEquatable<int32>[]. This function does that.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public static IEnumerable<T2> CastedEnumerable<T1, T2>(IEnumerable<T1> original) where T1 : T2
        {
            List<T2> newEnumeration = new List<T2>();

            foreach (T1 element in original)
            {
                newEnumeration.Add(element);
            }

            return newEnumeration;
        }

        /// <summary>
        /// Perform an action on eachelement of the enumerable
        /// </summary>
        public static void Do<T1>(this IEnumerable<T1> enumerable, Action<T1> action)
        {
            foreach (T1 element in enumerable) action(element);
        }

        /// <summary>
        /// Splits an IEnumerable into two IEnumerable based on a condition. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="condition"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static (IEnumerable<T> TrueValues, IEnumerable<T> FalseValues) SplitConditional<T>(
            this IEnumerable<T> target, Func<T, bool> condition)
        {
            List<T> trues = new List<T>();
            List<T> falses = new List<T>();

            foreach (T variable in target)
            {
                if(condition(variable))
                    trues.Add(variable);
                else
                    falses.Add(variable);
            }

            return (trues, falses);
        }

        public static T RandomElement<T>(this IEnumerable<T> target)
        {
            if (target is T[] array)
                return UTArray.RandomElement(array);
            
            return UTArray.RandomElement(target.ToArray());
        }
    }
}
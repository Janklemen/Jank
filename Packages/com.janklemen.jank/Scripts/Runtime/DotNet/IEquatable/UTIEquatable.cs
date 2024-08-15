using System;
using System.Collections.Generic;
using System.Linq;

namespace Jank.DotNet.IEquatable
{
    /// <summary>
    /// Utilities for IEquatable things
    /// </summary>
    public static class UTIEquatable
    {
        /// <summary>
        /// Takes two IEquatable collections and tests to see if they are equal
        /// without regard to order. That is, that each element in collection1 has
        /// an equal corresponding element in collection 2, and collection2 does not
        /// have any elements beyond those
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection1"></param>
        /// <param name="collection2"></param>
        /// <returns></returns>
        public static bool AreEqualUnorderedCollections<T>(IEnumerable<T> collection1, IEnumerable<T> collection2) where T:IEquatable<T>
        {
            if (collection1 == null && collection2 == null)
            {
                return true;
            }

            if (collection1 == null || collection2 == null)
            {
                return false;
            }

            IEnumerable<T> equatables = collection1 as T[] ?? collection1.ToArray();
            IEnumerable<T> enumerable = collection2 as T[] ?? collection2.ToArray();
            if (equatables.Count() != enumerable.Count())
            {
                return false;
            }

            List<T> first = new List<T>(equatables);
            List<T> second = new List<T>(enumerable);

            for (int i = first.Count - 1; i >= 0; i--)
            {
                int j = second.Count - 1;
                bool testResult = false;

                for (; j >= 0; j--)
                {
                    if (first[i].Equals(second[j]))
                    {
                        testResult = true;
                        break;
                    }
                }

                if (testResult == false)
                {
                    return false;
                }

                first.RemoveAt(i);
                second.RemoveAt(j);
            }

            return true;
        }


        /// <summary>
        /// Takes a collection of unordered IEquatables and counts how many values are shared
        /// with the other. In 1,2,2,3,4 and 2,2,2,3,4 : 4 values are shared. 2,2,3,4
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection1"></param>
        /// <param name="collection2"></param>
        /// <returns></returns>
        public static int SharedValuesCount<T>(IEnumerable<T> collection1, IEnumerable<T> collection2) where T: IEquatable<T>
        {
            if (collection1 == null || collection2 == null)
            {
                return 0;
            }

            List<T> second = new List<T>(collection2);

            int shared = 0;

            foreach (T val in collection1)
            {
                for (int j = 0; j < second.Count; j++)
                {
                    if (val.Equals(second[j]))
                    {
                        shared++;
                        second.RemoveAt(j);
                        break;
                    }
                }
            }

            return shared;
        }
    }
}

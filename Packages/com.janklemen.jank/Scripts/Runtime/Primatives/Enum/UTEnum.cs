﻿using System;
using System.Linq;
using Jank.DataStructures.Array;

namespace Jank.Primatives.Enum
{
    /// <summary>
    /// Utilities for using Enums
    /// </summary>
    public static class UtEnum
    {
        #region API
        /// <summary>
        /// Return an enum as an array of it's elements
        /// </summary>
        /// <typeparam name="TEnum">Enum</typeparam>
        /// <returns>Enum values as array</returns>
        public static TEnum[] GetEnumAsArray<TEnum>()
        {
            return System.Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
        }

        /// <summary>
        /// Returns a random element from an enum (This methods creates an array each time, if this needs doing a lot it's better to use GetEnumAsArray directly and get a Random elements from that instead). 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>Random Enum</returns>
        public static TEnum Random<TEnum>()
        {
            return GetEnumAsArray<TEnum>().RandomElement();
        }

        /// <summary>
        /// Based on enum order, provides the next enum following the input enum
        /// </summary>
        /// <typeparam name="TEnum">Enum</typeparam>
        /// <param name="currentValue">Get next value given current value</param>
        /// <returns>Next enum</returns>
        public static TEnum GetNext<TEnum>(this TEnum currentValue) where TEnum : IComparable
        {
            TEnum[] enums = GetEnumAsArray<TEnum>();

            int curValIndex = 0;

            for (int i = 0; i < enums.Length; i++)
            {
                if (enums[i].CompareTo(currentValue) == 0)
                {
                    curValIndex = i;
                    break;
                }
            }

            return enums[(curValIndex + 1) % enums.Length];
        }
        #endregion
    }
}
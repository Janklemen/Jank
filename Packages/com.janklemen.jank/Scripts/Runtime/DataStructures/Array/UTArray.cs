﻿using System;
using System.Linq;
using System.Text;
using Jank.Utilities.Random;
using Jank.Utilities.Vector;

namespace Jank.DataStructures.Array
{
    /// <summary>
    /// Contains utility methods and extensions for working with arrays
    /// </summary>
    public static class UTArray
    {
        /// <summary>
        /// Takes two arrays and combines them into a single array
        /// </summary>
        /// <typeparam name="T">array type</typeparam>
        /// <param name="array1">first array</param>
        /// <param name="array2">second array</param>
        /// <returns>single array containing elements in array1 then array2</returns>
        public static T[] Combine<T>(T[] array1, T[] array2)
        {
            T[] combined = new T[array1.Length + array2.Length];

            int index = 0;

            while (index < array1.Length)
            {
                combined[index] = array1[index++];
            }

            int appendIndex = 0;

            while (index < combined.Length)
            {
                combined[index++] = array2[appendIndex++];
            }

            return combined;
        }

        /// <summary>
        /// Cheecks if an array is equal to another array by checking each element
        /// based on index. Requires 
        /// </summary>
        /// <typeparam name="T">Array Type</typeparam>
        /// <param name="array1">first array</param>
        /// <param name="array2">second array</param>
        /// <returns>true if both arrays are same size, and each element passes a .Equals() test</returns>
        public static bool EqualsElementWise<T>(this T[] array1, T[] array2) where T : IEquatable<T>
        {
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (!array1[i].Equals(array2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Takes a 1D array and returns the minimum array length to create a square 
        /// 2D array that can contain all elements. 
        /// Example: An array length 4 returns 2, array length 6 return 3, array length 9 returns 3, etc. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int SquareSideLength<T>(this T[] array)
        {
            return (int)Math.Ceiling(Math.Sqrt(array.Length));
        }

        /// <summary>
        /// Copy an array, without copying the internals of contained classes. 
        /// This means that referecnes are copied. 
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="array">Array to copy</param>
        /// <returns>New shallow copy of an array</returns>
        public static T[] ShallowCopy<T>(this T[] array)
        {
            return array.ToArray();
        }

        /// <summary>
        /// Returns a random element in the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns>element</returns>
        public static T RandomElement<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                return default;
            
            return array[UTRandom.Int(array.Length)];
        }

        /// <summary>
        /// Constructs an array with each element created by calling constructionFuntion
        /// </summary>
        /// <typeparam name="T">array type</typeparam>
        /// <param name="length">length of array</param>
        /// <param name="constructionFunction">Function that constructs 1 T</param>
        /// <returns>constructed array</returns>
        public static T[] ConstructArray<T>(int length, Func<T> constructionFunction)
        {
            T[] array = new T[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = constructionFunction();
            }

            return array;
        }

        /// <summary>
        /// Construct an array using a function. The input to the function is the index of
        /// the array element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="length"></param>
        /// <param name="constructionFunction"></param>
        /// <returns></returns>
        public static T[] ConstructArray<T>(int length, Func<int, T> constructionFunction)
        {
            T[] array = new T[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = constructionFunction(i);
            }

            return array;
        }

        /// <summary>
        /// Construct an array using a function. The input to the function is the index of
        /// the array element and the array itself. 
        /// </summary>
        /// <typeparam name="T">array type</typeparam>
        /// <param name="length">length of array</param>
        /// <param name="constructionFunction">Function that constructs 1 T</param>
        /// <returns>constructed array</returns>
        public static T[] ConstructArray<T>(int length, Func<int, T[], T> constructionFunction)
        {
            T[] array = new T[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = constructionFunction(i, array);
            }

            return array;
        }

        /// <summary>
        /// Returns a portion of an array. End index is exclusive
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="target">Target Array</param>
        /// <param name="start">index to start at</param>
        /// <param name="end">index to end at (exclusive)</param>
        /// <returns>new array containing elements between subarray indices</returns>
        public static T[] SubArray<T>(this T[] target, int start, int end)
        {
            if (start < 0 || start >= end || end > target.Length)
            {
                throw new ArgumentException("Indicies invalid in sub array call");
            }

            int cutLength = end - start;
            T[] array = new T[cutLength];

            System.Array.Copy(target, start, array, 0, cutLength);
            return array;
        }

        /// <summary>
        /// Returns a simple string version of the array. 
        /// This is each element with a space between them. 
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="target">Array to target</param>
        /// <returns>All elements as strings, with " " character between them</returns>
        public static string AsSimpleString<T>(this T[] target)
        {
            StringBuilder sb = new();

            for(int i = 0; i < target.Length; i++)
            {
                sb.Append(target[i]);

                if(i != target.Length - 1)
                {
                    sb.Append(" ");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Checks if the target array contains all elements in the contains array
        /// </summary>
        /// <param name="target"></param>
        /// <param name="contains"></param>
        /// <returns></returns>
        public static bool ContainsElements<T>(this T[] target, T[] contains)
        {
            foreach(T el in contains)
            {
                if (!target.Contains(el)) return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if the array is null or contains no elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] target) => target == null || target.Length == 0;

        /// <summary>
        ///  Can be used to find things like max, min, etc. Selects the first element as the candidate.
        ///  Then performs query, first arg next element, second arg current candidate. If true, next element is set to the current candidate
        ///  After searching all elements returns the current candidate
        /// </summary>
        public static T CalcSearch<T>(this T[] target, Func<T, T, bool> query)
        {
            T candidate = target[0];

            for(int i = 1; i<target.Length; i++)
            {
                if (query(target[i], candidate)) candidate = target[i];
            }

            return candidate;
        }

        /// <summary>
        /// Mutates array elements with action
        /// </summary>
        public static void Mutate<T>(ref T[] target, Func<T, T> action)
        {
            for(int i = 0; i<target.Length; i++)
            {
                target[i] = action(target[i]);
            }
        }

        public static T At2DIndex<T>(this T[] target, DiscreteVector2 index, int xWidth)
        {
            return target[index.X + index.Y * xWidth];
        }
        
        public static void Set2DIndex<T>(this T[] target, DiscreteVector2 index, int xWidth, T newValue)
        {
            target[index.X + index.Y * xWidth] = newValue;
        }

        /// <summary>
        /// Given two arrays of equal size provide the value from the target array with the same index
        /// as the the key in the lookup array.
        /// </summary>
        public static bool TryParallelArrayLookup<T1, T2>(this T1[] target, T2[] lookup, T2 key, out T1 result)
        {
            int index = System.Array.IndexOf(lookup, key);
        
            if (index != -1)
            {
                result = target[index];
                return true;
            }

            result = default;
            return false;
        }

        public static int[] IndiciesAsArray<T>(this T[] array)
        {
            int[] indices = new int[array.Length];

            for (int i = 0; i < array.Length; i++)
                indices[i] = i;

            return indices;
        }

        /// <summary>
        /// Adds an element to an array by making a new array
        /// </summary>
        /// <param name="target"></param>
        /// <param name="newElement"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] Add<T>(this T[] target, T newElement)
        {
            T[] newArray = new T[target.Length];

            for (int i = 0; i < target.Length; i++)
                newArray[i] = target[i];

            newArray[target.Length] = newElement;
            return newArray;
        }

        /// <summary>
        /// Returns the modular next value following the given value.
        /// If the given value is not in the array returns the first element.
        /// If array is size 0 returns default
        /// </summary>
        /// <param name="target"></param>
        /// <param name="lastValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Next<T>(this T[] target, T lastValue)
        {
            if (target == null || target.Length == 0)
                return default;
            
            for (int i = 0; i < target.Length; i++)
                if (target[i].Equals(lastValue))
                    return target[(i + 1) % target.Length];

            return target[0];
        }

        public static int[] FromRange(Range range) => Enumerable.Range(range.Start.Value, range.End.Value).ToArray();

        public static T[] FromRange<T>(Range range, Func<int, T> constructor) 
            => FromRange(range).Select(i => constructor(i)).ToArray();

        public static bool ValidIndex<T>(this T[] target, int index) => index >= 0 && index < target.Length;

        public static void Reset<T>(this T[] allMoves, T val)
        {
            for (int i = 0; i < allMoves.Length; i++)
                allMoves[i] = val;
        }
    }
}
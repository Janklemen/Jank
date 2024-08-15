using System;
using UnityEngine;

namespace Jank.Utilities.Random
{
    /// <summary>
    /// Utilties for gerating random numbers
    /// </summary>
    public static class UTRandom
    {
        static Lazy<System.Random> _random = new(() => new System.Random());

        /// <summary>
        /// Increment the target value based on a given chance and an incrementer function.
        /// </summary>
        /// <typeparam name="T">The type of the target value.</typeparam>
        /// <param name="target">The target value to be incremented reference</param>
        /// <param name="chance">The chance of incrementing the target value (between 0 and 1).</param>
        /// <param name="incrementer">The incrementer function that takes the target value and returns the incremented value.</param>
        public static void ChanceIncrement<T>(this ref T target, float chance, Func<T, T> incrementer)
            where T : struct
        {
            if (Bool(chance))
                target = incrementer(target);
        }
        
        /// <summary>
        /// Increment the start and/or end value of a Range by one based on given chances and conditions
        /// </summary>
        /// <param name="range">The Range to be incremented</param>
        /// <param name="startChance">The chance of incrementing the start value (between 0 and 1)</param>
        /// <param name="endChance">The chance of incrementing the end value (between 0 and 1)</param>
        /// <param name="incrementOne">If true, only increment the start value when condition is met, otherwise, there is a change to increment both</param>
        /// <returns>A new Range with updated start and/or end values</returns>
        public static Range ChanceIncrementRange(this Range range, float startChance, float endChance, bool incrementOne)
        {
            Index start = range.Start;
            Index end = range.End;

            if (start.Value < end.Value && Bool(startChance))
            {
                start = new Index(start.Value + 1);

                if (incrementOne)
                    return new Range(start, end);
            }

            if (Bool(endChance))
                end = new Index(end.Value + 1);

            return new Range(start, end);
        }
        
        /// <summary>
        /// Returns a random number between 0 (inclusive) and 1 (exclusive)
        /// </summary>
        /// <returns>random number 0 - 1 exclusive of 1</returns>
        public static double Percentage()
        {
            return _random.Value.NextDouble();
        }

        /// <summary>
        /// Returns a random number between 0 (inclusive) and 1 (exclusive)
        /// </summary>
        /// <returns>random number 0 - 1 exclusive</returns>
        public static float PercentageF()
        {
            return (float)Percentage();
        }

        /// <summary>
        /// Tests float against a random percentage between 0 and 1. If float is greater than
        /// or equal to percentage, return true. Otherwise return false;
        /// </summary>
        /// <param name="test">float to test</param>
        /// <returns>result of random test</returns>
        public static bool TestPercentage(float test)
        {
            return test >= PercentageF();
        }

        /// <summary>
        /// Returns a float between 0 and max (inclusive)
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Float(float max = float.MaxValue)
        {
            return PercentageF() * max;
        }
        
        /// <summary>
        /// Returns an int between 0 and max (inclusive). Max must be > 0
        /// </summary>
        /// <param name="max">max int to random (inclusive)</param>
        /// <returns>random int between 0 and max (exclusive)</returns>
        public static int Int(int max = int.MaxValue)
        {
            return _random.Value.Next(max);
        }

        /// <summary>
        /// Returns an int between 0 and max (exclusive). Max must be > 0
        /// </summary>
        /// <param name="min">max int to random (inclusive)</param>
        /// <param name="max">max int to random (exclusive)</param>
        /// <returns>random int between min (inclusive) and max (exclusive)</returns>
        public static int Int(int min, int max)
        {
            return _random.Value.Next(min, max);
        }

        public static int Int(Range range)
        {
            return _random.Value.Next(range.Start.Value, range.End.Value);
        }

        /// <summary>
        /// Get a Bool with a percentage change of being true
        /// </summary>
        /// <param name="chanceTrue">number between 0 and 1 chance of returning true</param>
        /// <returns>random bool</returns>
        public static bool Bool(double chanceTrue = 0.5)
        {
            return Percentage() <= chanceTrue;
        }

        /// <summary>
        /// Returns a random byte array
        /// </summary>
        public static byte[] Bytes(int length)
        {
            byte[] bytes = new byte[length];
            _random.Value.NextBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Returns a random uppercase or lowercase letter
        /// </summary>
        public static char Letter()
        {
            // 50/50 upper or lower
            if (Bool())
            {
                int min = 'A';
                int max = 'Z';

                return (char)Int(min, max + 1);
            }
            else
            {
                int min = 'a';
                int max = 'z';

                return (char)Int(min, max + 1);
            }
        }

        public static Color Color(bool randomAlpha = false)
        {
            return new Color(
                UTRandom.Float(1f),
                UTRandom.Float(1f),
                UTRandom.Float(1f),
                randomAlpha ? UTRandom.Float(1f) : 1
            );
        }
    }
}
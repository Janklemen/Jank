﻿using Jank.Utilities.Hashing;

namespace Jank.Utilities.Vector
{
    /// <summary>
    /// A 2D coordinate with an x and y value where x and y are always integers. IMMUTABLE
    /// </summary>
    public struct DiscreteVector2
    {
        /// <summary>
        /// Make DiscreteVector2 with two int coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public DiscreteVector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// The X value of the coordinate
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// The Y value of the coordinate
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// results in all combinations of x and y. So x * y
        /// </summary>
        public int Product => X * Y;


        public static DiscreteVector2 Left => new(-1, 0);
        public static DiscreteVector2 Up => new(0, 1);
        public static DiscreteVector2 Right => new(1, 0);
        public static DiscreteVector2 Down => new(0, -1);
        public static DiscreteVector2 Zero => new(0, 0);


        /// <summary>
        /// Returns new GridCoordinate2 with x and y values summed
        /// </summary>
        /// <param name="vec1">First GridCoordinate2</param>
        /// <param name="vec2">Second GridCoordinate2</param>
        /// <returns>addition</returns>
        public static DiscreteVector2 operator +(DiscreteVector2 vec1, DiscreteVector2 vec2)
        {
            return new DiscreteVector2(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        /// <summary>
        /// Returns new GridCoordinate2 with x and y values subtracted.
        /// </summary>
        /// <param name="vec1">Minuhend</param>
        /// <param name="vec2">Subtrahend</param>
        /// <returns>difference</returns>
        public static DiscreteVector2 operator -(DiscreteVector2 vec1, DiscreteVector2 vec2)
        {
            return new DiscreteVector2(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }

        public static bool operator ==(DiscreteVector2 vec1, DiscreteVector2 vec2) => vec1.X == vec2.X && vec1.Y == vec2.Y;

        public static bool operator !=(DiscreteVector2 vec1, DiscreteVector2 vec2) => !(vec1.X == vec2.X && vec1.Y == vec2.Y);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is DiscreteVector2 vector && X == vector.X && Y == vector.Y;

        /// <inheritdoc />
        public override int GetHashCode() => UTHash.BasicHash(X, Y);

        public static implicit operator DiscreteVector2((int, int) tuple) => new(tuple.Item1, tuple.Item2);

        public override string ToString() => $"[ {X}, {Y} ]";

        public static DiscreteVector2 Parse(string str)
        {
            string[] args = str.Substring(1, str.Length - 2).Split(",");
            return (int.Parse(args[0]), int.Parse(args[1]));
        }
    }
}


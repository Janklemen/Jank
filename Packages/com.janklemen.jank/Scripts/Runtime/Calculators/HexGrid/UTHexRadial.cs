using System.Collections.Generic;

namespace Jank.Calculators.HexGrid
{
    /// <summary>
    /// <para>
    /// Standardizing hex grid representations helps to better manage hex girds and coordinates in data structures.
    /// This class provides functionality required for radial representations of discrete hex grids. Radial representation
    /// works by considering the hex "space" as surrounding a central origin (0,0,0). The size of the space is the
    /// radius required to contain all required coordinates. That means the further the coordinate is from origin the
    /// more space is required to contain the data. An index is determined by counting all hexes in a ring starting from
    /// coordinate (x=0, y=ring_radius) and moving clockwise. An index can broken into two pieces. The ringStep is
    /// the number of steps clockwise from the ring start index. The ringRadius is the distance of all indices in
    /// the ring from origin. 
    /// </para>
    /// <para>
    /// This technique makes it possible to easily convert between hex coordinates and 1D index in an array.
    /// </para>
    /// </summary>
    public static class UTHexRadial
    {
        /// <summary>
        /// Get the standard starting coordinate of the radial ring.
        /// </summary>
        /// <param name="ring"></param>
        /// <returns></returns>
        public static HexCoordinate RingStartHex(int ring) => new(0, ring);

        /// <summary>
        /// With the index, get the coordinate
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static HexCoordinate HexFromIndex(int index)
        {
            if (index == 0)
                return HexCoordinate.Zero;

            int ring = 1;
            int ringSize = 6;
            int totalCounted = 1;

            // find the ring
            while (index >= (totalCounted + ringSize))
            {
                ring++;
                totalCounted += ringSize;
                ringSize += 6;
            }

            // get the standard ring start
            HexCoordinate candidate = RingStartHex(ring);

            int ringStepsToTake = index - totalCounted;

            for (int i = 0; i < UTHexCoordinate.DirectionSteps.Length && ringStepsToTake > 0; i++)
            {
                EHexDirection dir = UTHexCoordinate.ClockWiseDirections[i];

                for (int steps = 0; steps < ring && ringStepsToTake > 0; steps++)
                {
                    candidate = candidate.Step(dir);
                    ringStepsToTake--;
                }
            }

            return candidate;
        }

        /// <summary>
        /// Starts at the standard ring start of a ring, then steps around to ring
        /// to find the target hex. Counts the steps taken to reach the target. returns -1
        /// if fails but should never fail.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int RingStep(HexCoordinate target)
        {
            if (target == HexCoordinate.Zero) return 0;

            int ring = target.DistanceFromOrigin();
            HexCoordinate cur = RingStartHex(ring);

            int index = 0;

            for (int i = 0; i < UTHexCoordinate.DirectionSteps.Length; i++)
            {
                EHexDirection dir = UTHexCoordinate.ClockWiseDirections[i];

                for (int steps = 0; steps < ring; steps++)
                {
                    if (cur == target)
                        return index;

                    cur = cur.Step(dir);
                    index++;
                }
            }

            // SHOULD NEVER HAPPEN
            return -1;
        }

        /// <summary>
        /// Starts at the standard ring start of a ring, then steps around the ring
        /// and records the coordinates
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static HexCoordinate[] CircleBorderHexes(int ring)
        {
            if (ring == 0) return new[] { HexCoordinate.Zero };

            List<HexCoordinate> coordinates = new List<HexCoordinate>();

            HexCoordinate cur = RingStartHex(ring);

            for (int i = 0; i < UTHexCoordinate.DirectionSteps.Length; i++)
            {
                EHexDirection dir = UTHexCoordinate.ClockWiseDirections[i];

                for (int steps = 0; steps < ring; steps++)
                {
                    coordinates.Add(cur);
                    cur = cur.Step(dir);
                }
            }

            return coordinates.ToArray();
        }

        /// <summary>
        /// Returns all coorindates for all the rings in standard radial order for a number of rings
        /// </summary>
        /// <param name="rings"></param>
        /// <returns></returns>
        public static HexCoordinate[] CircleFilledHexes(int rings)
        {
            List<HexCoordinate> coordinates = new List<HexCoordinate>();
            coordinates.Add(HexCoordinate.Zero);

            for (int ring = 0; ring <= rings; ring++)
            {
                HexCoordinate cur = RingStartHex(ring);

                for (int i = 0; i < UTHexCoordinate.DirectionSteps.Length; i++)
                {
                    EHexDirection dir = UTHexCoordinate.ClockWiseDirections[i];

                    for (int steps = 0; steps < ring; steps++)
                    {
                        coordinates.Add(cur);
                        cur = cur.Step(dir);
                    }
                }
            }

            return coordinates.ToArray();
        }

        /// <summary>
        /// Returns the starting index nof a ring if the ring lives in a 1D array contaiing all other rings before it.
        /// </summary>
        /// <param name="ring"></param>
        /// <returns></returns>
        public static int RingStartIndex(int ring)
        {
            if (ring == 0) return 0;
            if (ring == 1) return 1;
            if (ring == 2) return 7;
            if (ring >= 3) return ((ring - 1) * 6 + 6) / 2 * (ring - 1) + 1;

            return -1;
        }

        /// <summary>
        /// The number of hexes in the ring
        /// </summary>
        /// <param name="ring"></param>
        /// <returns></returns>
        public static int RingHexCount(int ring)
        {
            if (ring == 0) return 1;
            return ring * 6;
        }

        /// <summary>
        /// Index of the coordinate in a flat array
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int RadialIndex(this HexCoordinate target)
        {
            int ring = target.DistanceFromOrigin();

            int ringStartIndex = RingStartIndex(ring);
            int ringIndex = RingStep(target);

            return ringStartIndex + ringIndex;
        }

        /// <summary>
        /// Calculates the number of the hexes with rings around origin
        /// </summary>
        /// <param name="rings"></param>
        /// <returns></returns>
        public static int TotalHexesInRadial(int rings)
        {
            int total = 0;

            for (int i = 0; i <= rings; i++)
                total += RingHexCount(i);

            return total;
        }
    }
}
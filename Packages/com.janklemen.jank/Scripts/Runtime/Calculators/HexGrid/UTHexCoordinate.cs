namespace Jank.Calculators.HexGrid
{
    /// <summary>
    /// Provides calculations related to HexCoordinates.
    /// </summary>
    public static class UTHexCoordinate
    {
        /// <summary>
        /// Holds direction HexCoordinate in order of EDirection array
        /// </summary>
        public static readonly HexCoordinate[] DirectionSteps
            = { new(-1, 0), new(0, -1), new(1, -1), new(1, 0), new(0, 1), new(-1, 1) };

        /// <summary>
        /// Assumed directions to take one after the other when stepping clockwise around the
        /// hex. It is possible for this to be wrong if x, y, z are not assumed that same as images in comments
        /// </summary>
        public static readonly EHexDirection[] ClockWiseDirections =
        {
            EHexDirection.NegXPosZ,
            EHexDirection.PosZNegY,
            EHexDirection.NegYPosX,
            EHexDirection.PosXNegZ,
            EHexDirection.NegZPosY,
            EHexDirection.PosYNegX
        };

        /// <summary>
        /// Get HexCoordinate that represents a step in a direction
        /// </summary>
        public static HexCoordinate Step(this HexCoordinate target, EHexDirection hexDirection)
            => target + DirectionSteps[(int)hexDirection];


        /// <summary>
        /// Distance heuristic that works with hex grid. It's equivalent to manhattan distance of a cube grid.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="opposite"></param>
        /// <returns></returns>
        public static int Distance(this HexCoordinate target, HexCoordinate opposite)
        {
            return (System.Math.Abs(target.X - opposite.X) + System.Math.Abs(target.Y - opposite.Y) +
                    System.Math.Abs(target.Z - opposite.Z)) / 2;
        }
        
        /// <summary>
        /// Return the distance from origin [0,0,0] 
        /// </summary>
        public static int DistanceFromOrigin(this HexCoordinate target) => HexCoordinate.Zero.Distance(target);

    }
}
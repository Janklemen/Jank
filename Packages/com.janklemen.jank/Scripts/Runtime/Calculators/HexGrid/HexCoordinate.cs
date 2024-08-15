using Jank.Utilities.Hashing;

namespace Jank.Calculators.HexGrid
{
    /// <summary>
    /// <para>Look down this article: https://catlikecoding.com/unity/tutorials/hex-map/part-1/.
    /// Also https://www.redblobgames.com/grids/hexagons/ </para>
    /// <para>
    /// Basically, <see cref="HexCoordinate"/> use a 3D system of dependent axis that allow 6 directions to be 
    /// expressed easily (in mathematical terms). The coordinates work using x,y,z where each dimension
    /// represents distance from an axis along a natural hex axis. See image here:
    /// https://catlikecoding.com/unity/tutorials/hex-map/part-1/hexagonal-coordinates/cube-diagram.png . 
    /// Only two axes are required to infer the third, as such the struct is created using only 2
    /// variables. 
    /// </para>
    /// <para>
    /// Based on terminology in the redblobgames website above, this struct holds axial coordinates and
    /// has the Z function, which allows calculation of the final coordinate required to convert
    /// Axial -> Cube coordinates.  
    /// </para>
    /// <para>
    /// To help simplify things, I tend to think of the hex grid as a pointed-side up and down, flat-side
    /// left and right configuration. This means that there is a straight X-axis horizontally through the center
    /// of the grid (which as a human is easy because we like horizons). For the X axis, going up is positive and
    /// down is negative. The Z axis is the diagonal going top left to bottom right with downleft being positive.
    /// The Y axis is the diagonal going top right to bottom left with downright being positive.  
    /// </para>
    /// </summary>
    public struct HexCoordinate
    {
        /// <summary>
        /// X coordinate, input on construction
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y coordinate, input on construction
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Z coordinate, calculated from x y
        /// </summary>
        public int Z { get; private set; }
        
        /// <summary>
        /// Constructor. Uses only x and y. z is inferred
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public HexCoordinate(int x, int y)
        {
            X = x;
            Y = y;
            Z = -(x + y);
        }

        /// <summary>
        /// Construct from x and z instead of x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static HexCoordinate XZConstruct(int x, int z)
        {
            return new HexCoordinate(x, -(x + z));
        }

        /// <summary>
        /// construct from y and z instead of x and y
        /// </summary>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static HexCoordinate YZConstruct(int y, int z)
        {
            return new HexCoordinate(-(y + z), y);
        }

        /// <summary>
        /// Zero Vector
        /// </summary>
        public static HexCoordinate Zero => new(0, 0);
        
        /// <summary>
        /// The sum of all three dimensions. Always 0 in a valid coordinate
        /// </summary>
        /// <returns></returns>
        public int SumAbsolute => System.Math.Abs(X) + System.Math.Abs(Y) + System.Math.Abs(Z);
        
        /// <summary>
        /// Returns new GridCoordinate2 with x and y values summed
        /// </summary>
        /// <param name="vec1">First GridCoordinate2</param>
        /// <param name="vec2">Second GridCoordinate2</param>
        /// <returns>addition</returns>
        public static HexCoordinate operator +(HexCoordinate vec1, HexCoordinate vec2)
        {
            return new HexCoordinate(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        /// <summary>
        /// Returns new GridCoordinate2 with x and y values subtracted.
        /// </summary>
        /// <param name="vec1">Minuhend</param>
        /// <param name="vec2">Subtrahend</param>
        /// <returns>difference</returns>
        public static HexCoordinate operator -(HexCoordinate vec1, HexCoordinate vec2)
        {
            return new HexCoordinate(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }

        /// <inheritdoc />
        public static bool operator ==(HexCoordinate vec1, HexCoordinate vec2) => vec1.X == vec2.X && vec1.Y == vec2.Y;

        /// <inheritdoc />
        public static bool operator !=(HexCoordinate vec1, HexCoordinate vec2) => !(vec1.X == vec2.X && vec1.Y == vec2.Y);
        
        /// <inheritdoc />
        public override string ToString() => $"[{X}, {Y}, {Z}]";
        
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is HexCoordinate vector && X == vector.X && Y == vector.Y;

        /// <inheritdoc />
        public override int GetHashCode() => UTHash.BasicHash(X, Y);

    }
}
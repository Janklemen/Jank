using UnityEngine;

namespace Jank.Calculators.HexGrid
{
    public static class UTHexGridDisplay
    {
        public static Vector2 Solve(HexGridDisplayOptions options, HexCoordinate coords)
        {
            return new Vector2(
                (coords.Y - coords.Z) / 2f * options.HorizontalSpacing,
                coords.X * options.VerticalSpacing
            );
        }
    }
}
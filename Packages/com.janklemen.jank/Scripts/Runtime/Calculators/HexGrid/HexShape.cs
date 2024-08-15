

// A local space shape with pivot 0,0
namespace Jank.Calculators.HexGrid
{
    public struct HexShape
    {
        public HexCoordinate[] Hexes { get; private set; }
        public HexShape(HexCoordinate[] hexes) => this.Hexes = hexes;
    }
}

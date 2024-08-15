namespace Jank.Calculators.HexGrid
{
    public class HexValuePair<T>
    {
        public HexCoordinate Hex;
        public T Value;

        public HexValuePair(HexCoordinate hex, T value)
        {
            this.Hex = hex;
            Value = value;
        }
    }
}
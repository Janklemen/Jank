namespace Jank.Calculators.HexGrid
{
    public struct HexGridDisplayOptions
    {
        public readonly float VerticalSpacing;
        public readonly float HorizontalSpacing;

        public HexGridDisplayOptions(float verticalSpacing, float horizontalSpacing)
        {
            VerticalSpacing = verticalSpacing;
            HorizontalSpacing = horizontalSpacing;
        }
    }
}
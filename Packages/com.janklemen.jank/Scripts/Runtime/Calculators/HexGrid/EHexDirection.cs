namespace Jank.Calculators.HexGrid
{
    /// <summary>
    /// Direction are defined as a positive movement away from an axis,
    /// then a left or right choice
    /// </summary>
    public enum EHexDirection
    {
        NegXPosZ = 0,
        PosZNegY = 1,
        NegYPosX = 2,
        PosXNegZ = 3,
        NegZPosY = 4,
        PosYNegX = 5,
    }
}
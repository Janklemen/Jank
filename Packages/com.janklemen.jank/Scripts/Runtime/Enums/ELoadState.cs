namespace Jank.Enums
{
    /// <summary>
    /// Represents the status of a visual controller. If not Ready, the api of a controller will have undefined
    /// behaviour.
    /// </summary>
    public enum ELoadState
    {
        Loaded,
        Unloaded
    }
}
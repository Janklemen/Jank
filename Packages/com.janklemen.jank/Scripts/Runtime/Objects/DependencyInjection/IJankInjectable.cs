namespace Jank.Objects
{
    /// <summary>
    /// Pulls dependencies from an <see cref="ObjectManager"/> when <see cref="Inject"/> is called.
    /// </summary>
    public interface IJankInjectable
    {
        /// <summary>
        /// Pull dependencies from an object manager
        /// </summary>
        /// <param name="manager"></param>
        void Inject(IObjectManager manager);
    }
}
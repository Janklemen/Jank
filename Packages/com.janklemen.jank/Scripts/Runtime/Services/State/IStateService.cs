using Jank.Serialization;

namespace Jank.Services.State
{
    /// <summary>
    /// A service that allows the game state to be pulled. The game state is an <see cref="IJankSerializable"/> which
    /// allows it to be used in system like <see cref="SaveLoadService"/>.
    /// </summary>
    public interface IStateService
    {
        /// <summary>
        /// Get the state object. To be used the state object must be cast into it's concrete type. The concrete type
        /// is determined by the implementation.
        /// </summary>
        /// <returns>The state</returns>
        public IJankSerializable GetState();
    }
}
using Jank.Serialization;
using UnityEngine;

namespace Jank.Services.State
{
    /// <summary>
    /// A scriptable object state service
    /// </summary>
    public abstract class AStateService : ScriptableObject, IStateService
    {
        /// <inheritdoc />
        public abstract IJankSerializable GetState();
    }
}
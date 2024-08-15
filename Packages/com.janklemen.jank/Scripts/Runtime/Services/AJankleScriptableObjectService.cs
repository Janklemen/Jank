using UnityEngine;

namespace Jank.Services
{
    /// <summary>
    /// Many services in the Jank framework are scriptable object services, which makes them easy to edit and inspect
    /// in the Unity Editor.
    /// </summary>
    public abstract class AJankleScriptableObjectService : ScriptableObject
    {
        /// <summary>
        /// Should be called before using a service to initialize it.
        /// </summary>
        public virtual void InitializeService() { }
    }
}

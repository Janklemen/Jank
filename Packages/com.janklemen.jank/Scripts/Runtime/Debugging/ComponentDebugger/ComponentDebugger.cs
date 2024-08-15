using Jank.Utilities;
using UnityEngine;

namespace Jank.Debugging.ComponentDebugger
{
    /// <summary>
    /// The ComponentDebugger class provides static methods to add debug information to a GameObject.
    /// </summary>
    public static class ComponentDebugger
    {
        /// <summary>
        /// Adds a GameObject to the specified origin GameObject.
        /// </summary>
        /// <param name="origin">The origin GameObject to add the GameObject to.</param>
        /// <param name="add">The GameObject to add to the origin.</param>
        public static void AddGameObject(GameObject origin, GameObject add)
        {
            DebugComponent debugger = origin.GetOrAddComponent<DebugComponent>();
            
            if(!debugger.GameObjects.Contains(add))
                debugger.GameObjects.Add(add);
        }

        /// <summary>
        /// Adds a string to the DebugComponent of a GameObject.
        /// </summary>
        /// <param name="origin">The GameObject to add the string to.</param>
        /// <param name="add">The string to be added to the DebugComponent.</param>
        public static void AddString(GameObject origin, string add)
        {            
            DebugComponent debugger = origin.GetOrAddComponent<DebugComponent>();
            
            if(!debugger.Strings.Contains(add))
                debugger.Strings.Add(add);
        }
    }
}

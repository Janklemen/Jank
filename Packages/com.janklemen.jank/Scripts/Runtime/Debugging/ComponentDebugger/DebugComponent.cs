using System.Collections.Generic;
using UnityEngine;

namespace Jank.Debugging.ComponentDebugger
{
    /// <summary>
    /// If attached to a GameObject, can be populated to allow visualization of information in the Editor.
    /// </summary>
    public class DebugComponent : MonoBehaviour
    {
        public List<GameObject> GameObjects = new();
        public List<string> Strings = new();
    }
}

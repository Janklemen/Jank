#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Jank.Debugging
{
    public static class UTDebugging
    {
        public static bool NullCheckAndSelectOnFail(this object toCheck, string name, GameObject gameobject)
        {
            if (toCheck == null)
            {
                Debug.LogError($"{name} was null on object {gameobject}");
#if UNITY_EDITOR
                Selection.SetActiveObjectWithContext(gameobject, gameobject);
#endif
                return true;
            }

            return false;
        }
    }
}
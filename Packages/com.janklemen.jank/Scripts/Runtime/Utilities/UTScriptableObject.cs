using UnityEngine;

namespace Jank.Utilities
{
    public static class UTScriptableObject
    {
        public static T InstantiateIfScriptableObject<T>(T instance)
        {
            if (instance is ScriptableObject)
                return (T) (object) Object.Instantiate(instance as ScriptableObject);
            else
                return instance;
        }
    }
}
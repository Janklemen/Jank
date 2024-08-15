#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

namespace Jank.Props.Utilities
{
    public class UTPropMenus
    {
        public static void CreateObject(string name)
        {
            string prefabPath = $"Packages/com.janklemen.Jank/Prefabs/Props/Library/{name}.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
            GameObject token = UnityEngine.Object.Instantiate(prefab);
            token.name = token.name.Remove(token.name.IndexOf("(Clone)", StringComparison.Ordinal));
        }
    }
}
#endif

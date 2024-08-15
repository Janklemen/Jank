using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Jank.Engine
{
    public static class UTJankEditor
    {
        #if UNITY_EDITOR
        static Dictionary<Action, EditorApplication.CallbackFunction> m_RegisteredFunctions = new();

        public static void RegisterEditorUpdateCallback(Action action)
        {
            if (!m_RegisteredFunctions.TryGetValue(action, out EditorApplication.CallbackFunction func))
            {
                func = () => action();
                m_RegisteredFunctions.Add(action, func);
            }

            EditorApplication.update += func;
        }

        public static void UnregisterEditorUpdateCallback(Action action)
        {
            if (!m_RegisteredFunctions.TryGetValue(action, out EditorApplication.CallbackFunction func))
                return;

            EditorApplication.update -= func;
        }
        #endif

        public static bool IsUnityEditor()
        {
#if UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
        
        public static bool IsUnityEditorPlayMode()
        {
#if UNITY_EDITOR
            return EditorApplication.isPlaying;
#else
            return false;
#endif
        }

        public static bool IsUnityEditorEditMode()
        {
#if UNITY_EDITOR
            return !EditorApplication.isPlaying;
#else
            return false;
#endif
        }
    }
}
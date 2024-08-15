using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
#endif

namespace Jank.Engine
{
    public class EngineHooks : MonoBehaviour
    {
        public static void RegisterUpdateCallback(Action callback)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                UTJankEditor.RegisterEditorUpdateCallback(callback);
                return;
            }
#endif

            Instance.RegisterOnUpdate(callback);
        }

        public static void UnregisterUpdateCallback(Action callback)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                UTJankEditor.UnregisterEditorUpdateCallback(callback);
                return;
            }
#endif

            Instance.UnregisterOnUpdate(callback);
        }

        public static async UniTask<float> AwaitNextFrame()
        {
#if UNITY_EDITOR
            const int millis = 1000 / 60;
            
            if (!EditorApplication.isPlaying)
            {
                Stopwatch st = new();
                st.Start();
                await Task.Delay(millis);
                st.Stop();
                return (float)st.Elapsed.TotalSeconds;
            }
#endif
            await UniTask.NextFrame();
            return Time.deltaTime;
        }

        static EngineHooks _instance;

        public static EngineHooks Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject parent = new("EngineHooks", typeof(EngineHooks));
                    DontDestroyOnLoad(parent);
                    parent.transform.SetSiblingIndex(0);
                    _instance = parent.GetComponent<EngineHooks>();
                }

                return _instance;
            }
        }

        readonly List<Action> _onUpdate = new();

        public void RegisterOnUpdate(Action action) => _onUpdate.Add(action);
        public void UnregisterOnUpdate(Action action) => _onUpdate.Remove(action);

        void Update()
        {
            foreach (Action action in _onUpdate)
                action();
        }
    }
}
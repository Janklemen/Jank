using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jank.Utilities
{
    public static class UTGameObject
    {
        public static T GetOrAddComponent<T>(this GameObject target) where T : Component
        {
            T component = target.GetComponent<T>();

            if (component == null)
                return target.AddComponent<T>();

            return component;
        }

        public static void BFS(this GameObject target, Action<GameObject> process)
        {
            Queue<GameObject> processing = new();
            processing.Enqueue(target);

            while (processing.Count > 0)
            {
                GameObject next = processing.Dequeue();

                foreach (Transform t in next.transform)
                    processing.Enqueue(t.gameObject);

                process(next);
            }
        }
    }
}
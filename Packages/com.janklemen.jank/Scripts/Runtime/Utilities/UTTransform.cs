using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jank.Utilities
{
    public static class UTTransform
    {
        public static void ProcessHierarchy(this Transform parent, Action<Transform> process)
        {
            Queue<Transform> queue = new Queue<Transform>(new[] {parent});

            while (queue.Count > 0)
            {
                Transform processing = queue.Dequeue();

                foreach (Transform ptrans in processing)
                    queue.Enqueue(ptrans);

                process(processing);
            }
        }

        public static void Replace(this Transform target, ref Transform replacing)
        {
            target.SetParent(replacing, false);
            target.SetParent(replacing.parent, true);
            UnityEngine.Object.Destroy(replacing.gameObject);
            replacing = target;
        }
    }
}
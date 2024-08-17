using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jank.Utilities
{
    public static class UTComponent
    {
        public static IEnumerable<T> GetComponentsInScene<T>()
            where T : Component
        {
            return SceneManager.GetActiveScene()
                .GetRootGameObjects()
                .SelectMany(gameObject => gameObject.GetComponentsInChildren<T>(true));
        }
    }
}
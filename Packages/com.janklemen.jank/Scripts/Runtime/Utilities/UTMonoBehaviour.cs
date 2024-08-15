using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jank.Utilities
{
    public static class UTMonoBehaviour
    {
        public static IEnumerable<MonoBehaviour> GetSceneRootMonoBehaviours() 
            => SceneManager.GetActiveScene()
                .GetRootGameObjects()
                .SelectMany(g => g.GetComponents<MonoBehaviour>());

        /// <summary>
        /// Destroys all non-tracked child objects of the target MonoBehaviour.
        /// </summary>
        /// <param name="target">The MonoBehaviour whose non-tracked children will be destroyed.</param>
        /// <param name="tracked">An IEnumerable of MonoBehaviours that represent the tracked objects.</param>
        public static void DestroyUntrackedChildren(this MonoBehaviour target, IEnumerable<MonoBehaviour> tracked)
        {
            Transform[] testTransforms = tracked.Select(mo => mo.transform).ToArray();
                
            foreach (Transform t in target.transform)
            {
                if(!testTransforms.Contains(t))
                    Object.Destroy(t.gameObject);
            }
        }
        
        
        
    }
}
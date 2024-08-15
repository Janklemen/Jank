using UnityEngine;

namespace Jank.Utilities
{
    public class DestroyIfPlaying : MonoBehaviour
    {
        void Awake()
        {
            Destroy(gameObject);
        }
    }
}
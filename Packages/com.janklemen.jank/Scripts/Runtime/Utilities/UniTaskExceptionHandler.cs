using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jank.Utilities
{
    public class UniTaskExceptionHandler : MonoBehaviour
    {
        void Awake()
        {
            UniTaskScheduler.UnobservedTaskException += (e) =>
            {
                Debug.LogException(e);
            };
        }
    }
}

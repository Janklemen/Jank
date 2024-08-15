using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jank.App
{
    /// <summary>
    /// Used by a <see cref="AJankContainer"/> to provide a run function that runs the app
    /// after dependency setup. This split was done to allow a single JankApp prefab
    /// with dependencies setup to run apps in different circumstances. For example,
    /// you might want the same dependencies for the main scene and a training scene.
    /// Both these scenes would have the same <see cref="AJankContainer"/> but different
    /// runners.
    /// </summary>
    public abstract class AJankRunner : MonoBehaviour
    {
        /// <summary>
        /// Run the application. This should be called from an <see cref="AJankContainer"/> which wraps it in a
        /// try/catch that understands how to react to <see cref="AppSignals"/>
        /// </summary>
        /// <param name="signals"></param>
        /// <returns></returns>
        public abstract UniTask Run();
    }
}
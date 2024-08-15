using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Jank.Props.Architecture;
using Jank.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jank.Props
{
    /// <summary>
    /// Makes some stuff nicer when making scenes to build and test visual controllers
    /// </summary>
    public abstract class APropTestScene : MonoBehaviour
    {
        protected UniTask UnloadLoadables()
        {
            foreach (MonoBehaviour behaviour in UTMonoBehaviour.GetSceneRootMonoBehaviours())
            {
                if (behaviour is ILoadable viz)
                    viz.Unload();
            }

            return UniTask.CompletedTask;
        }
        
        protected async UniTask LoadProp<T>(Func<T> dataFactory, bool setInteractable)
        {
            foreach (MonoBehaviour mono in SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g => g.GetComponents<MonoBehaviour>()))
            {
                if (mono is ILoadable<T> visual)
                    await visual.LoadWithData(dataFactory());

                if (mono is IInteractable inter)
                    await inter.SetInteractable(setInteractable);
            }
        }
    }
}

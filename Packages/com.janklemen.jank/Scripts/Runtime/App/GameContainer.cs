#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Jank.Objects;
using Jank.Services;
using Jank.Utilities;
using UnityEngine;

namespace Jank.App
{
    /// <summary>
    /// Responsible for bootstrapping an instance of the application by setting up dependencies and running the app
    /// in a try/catch that reacts to app level events (like quitting) via cancellation token.
    /// </summary>
    /// <remarks>
    /// This is should be attached to a <see cref="MonoBehaviour"/> that is the parent of some hierarchy that represents
    /// the game. All game logic should be run beneath this parent, and all game related objects that are instantiated
    /// should be instantiated beneath this. Conceptually, anything that is a sibling or parent of the container is in
    /// global space.
    /// </remarks>
    public class GameContainer : MonoBehaviour
    {
        [Header("Features")]
        [SerializeField]
        [Tooltip("The run function of this will be called once dependencies are set up")]
        AJankRunner Runner;

        [SerializeField]
        UnityEngine.Object[] Services;

        protected CancellationTokenSource QuitSource = new();
        
        void Awake()
        {
#if UNITY_EDITOR
            Application.runInBackground = true;
#endif
        }

        async UniTask Start()
        {
            // Clean objects from scene that are just there to make authoring easier
            gameObject.BFS(g =>
            {
                if(g.GetComponent<DestroyOnContainerLoad>())
                    Destroy(g);
            });
            
            // Make an object manager so that objects can be created with these dependencies
            ObjectManager obm = new(transform);
            obm.RegisterSingle(new AppSignals(QuitSource));

            if (Services != null)
            {
                foreach (object service in Services)
                    obm.RegisterSingle(service);
            }

            obm.ProcessGameObject(gameObject);

            if (Services != null)
            {
                foreach (object service in Services)
                {
                    if(service is AJankleScriptableObjectService jankleService)
                        jankleService.InitializeService();
                }
            }

            foreach (IRunOnContainerLoad loader in Services.Where(s => s is IRunOnContainerLoad))
                await loader.RunLoadOperation(obm);
            
            try
            {
                await Runner.Run();
            }
            catch (OperationCanceledException e)
            {
                if (e.CancellationToken == QuitSource.Token)
                {
#if UNITY_EDITOR
                    EditorApplication.ExitPlaymode();
#endif
                    Application.Quit();
                }
                else
                {
                    Debug.LogException(e);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
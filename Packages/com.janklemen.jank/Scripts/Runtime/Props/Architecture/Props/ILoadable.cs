using Cysharp.Threading.Tasks;
using Jank.Enums;
using Jank.Objects;
using Jank.Observables.Observables;
using Jank.Utilities;

namespace Jank.Props.Architecture
{
    public interface ILoadable : IManageableObject
    {
        /// <summary>
        /// Used to determine if the controllers APIs will act as expected.
        /// </summary>
        ELoadState LoadState { get; }

        /// <summary>
        /// Invoked after a full load has occured
        /// </summary>
        /// <returns></returns>
        IObservableAsync<Unit> ObserveLoad();
        
        /// <summary>
        /// Load the visual
        /// </summary>
        UniTask Load();

        /// <summary>
        /// Invoked just before unloading starts
        /// </summary>
        IObservableAsync<Unit> ObserveUnload();
        
        /// <summary>
        /// Unload the visual
        /// </summary>
        UniTask Unload();
    }
    
    /// <summary>
    /// <see cref="ILoadable{TLoadData}"/> sit on the parent of some hierarchy. They provide a way for a visual
    /// to be loaded with data. They are manageable objects, so they can be recycled. The clear method should be used to
    /// disable all visuals on the object so that it can be recycled later. The <see cref="ELoadState"/> can be used to
    /// determine if the controller is ready to be controlled
    /// </summary>
    /// <typeparam name="TLoadData"></typeparam>
    public interface ILoadable<TLoadData> : ILoadable
    {
        TLoadData LoadData { get; set; }
        
        /// <summary>
        /// Loads the visual based on the data. This could be a sync or async process depending on the complexity of the
        /// visuals to load.
        /// </summary>
        /// <param name="data">The data representing the initial state of the controller</param>
        /// <returns></returns>
        UniTask LoadWithData(TLoadData data);
    }
}
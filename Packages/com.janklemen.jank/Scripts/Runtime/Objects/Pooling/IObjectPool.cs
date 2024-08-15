using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jank.Objects
{
    public interface IObjectPool
    {
        /// <summary>
        /// Creates or recycles an object. Supplied objects should be <see cref="ObjectManager.Return{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of object to supply</typeparam>
        /// <returns>An instance of T that has been processed</returns>
        UniTask<T> Supply<T>() where T : IManageableObject, new();

        /// <summary>
        /// Instantiates a GameObject for you, or provides an unused on. To get rid of the object, it must be <see cref="ObjectManager.Return{T}"/>>
        /// </summary>
        /// <param name="prefab">The prefab that should be used to instantiate</param>
        /// <returns>A <see cref="GameObject"/> instance that has been processed</returns>
        UniTask<GameObject> Supply(GameObject prefab);

        /// <summary>
        /// Instantiates and immediately returns an instance of a prefab without trying to recycle
        /// to preallocate a single object
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        UniTask<GameObject> Prewarm(GameObject prefab);
        
        /// <summary>
        /// Returns an object that was previously <see cref="ObjectManager.Supply{T}"/> so that it can be recycled.
        /// </summary>
        /// <param name="obj">The instance to return</param>
        /// <typeparam name="T">Type of returned instance</typeparam>
        /// <exception cref="Exception">Thrown if the object is not present in the used list, meaning that it was
        /// not <see cref="ObjectManager.Supply{T}"/> </exception>
        void Return<T>(T obj) where T : IManageableObject, new();

        /// <summary>
        /// Return a previously <see cref="ObjectManager.Supply"/> <see cref="GameObject"/> to the <see cref="ObjectManager"/> so that
        /// it can disabled it and use it later. 
        /// </summary>
        /// <param name="obj">The instance of a GameObject to be returned</param>
        /// <exception cref="Exception">Thrown if the object is not present in the used list, meaning that it was
        /// not <see cref="ObjectManager.Supply"/></exception>
        void Return(GameObject obj);
    }
}
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jank.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Jank.Objects
{
    public class ObjectPool : IObjectPool
    {
        static object _instanceLock = new();
        static ObjectPool _instance = default;

        readonly Dictionary<Type, List<IManageableObject>> _pool = new();
        readonly Dictionary<Type, List<IManageableObject>> _used = new();

        readonly Dictionary<GameObject, List<GameObject>> _goPool = new();
        readonly Dictionary<GameObject, List<GameObject>> _goUsed = new();

        readonly GameObject _goPoolParent;

        public static ObjectPool Instance
        {
            get
            {
                if (_instance == default)
                {
                    lock (_instanceLock)
                    {
                        _instance ??= new ObjectPool();
                    }
                }

                return _instance;
            }
        }

        ObjectPool()
        {
            _goPoolParent = new("_ObjectPool");
            _goPoolParent.transform.SetSiblingIndex(0);
            Object.DontDestroyOnLoad(_goPoolParent);
        }

        /// <summary>
        /// Creates or recycles an object. Supplied objects should be <see cref="Return{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of object to supply</typeparam>
        /// <returns>An instance of T</returns>
        public async UniTask<T> Supply<T>() where T : IManageableObject, new()
        {
            // Each type gets its own pool. If a pool is not available make one
            if (!_pool.ContainsKey(typeof(T)))
            {
                _pool[typeof(T)] = new();
                _used[typeof(T)] = new();
            }

            if (_pool[typeof(T)].Count == 0)
            {
                // if the pool is empty, make a new instance and process it
                T instance = new();
                _used[typeof(T)].Add(instance);
                return instance;
            }
            else
            {
                // if their is an object that can be recycled, do so
                IManageableObject instance = _pool[typeof(T)][0];
                _pool[typeof(T)].RemoveAt(0);
                _used[typeof(T)].Add(instance);
                await instance.Clear();
                return (T) instance;
            }
        }

        /// <summary>
        /// Instantiates a GameObject for you, or provides an unused on. To get rid of the object, it must be <see cref="Return{T}"/>>
        /// </summary>
        /// <typeparam name="T">The type of object to supply</typeparam>
        /// <param name="prefab">The prefab that should be used to instantiate</param>
        /// <returns>A <see cref="GameObject"/> instance</returns>
        public async UniTask<GameObject> Supply(GameObject prefab)
        {
            AssurePoolLists(prefab);

            if (_goPool[prefab].Count == 0)
            {
                GameObject instance = UnityEngine.Object.Instantiate(prefab);
                _goUsed[prefab].Add(instance);
                return instance;
            }
            else
            {
                GameObject instance = _goPool[prefab][0];
                _goPool[prefab].RemoveAt(0);
                _goUsed[prefab].Add(instance);
                await ClearInstance(instance);
                return instance;
            }
        }

        void AssurePoolLists(GameObject prefab)
        {
            if (!_goPool.ContainsKey(prefab))
            {
                _goPool[prefab] = new();
                _goUsed[prefab] = new();
            }
        }

        static async UniTask ClearInstance(GameObject instance)
        {
            await instance.GetComponents<IManageableObject>()
                .ForEachAsync(async m => await m.Clear());
        }

        public async UniTask<GameObject> Prewarm(GameObject prefab)
        {
            AssurePoolLists(prefab);

            GameObject instance = UnityEngine.Object.Instantiate(prefab);
            _goPool[prefab].Add(instance);
            await ClearInstance(instance);
            OrganizeReturnedInScene(instance);
            return instance;
        }

        /// <summary>
        /// Returns an object that was previously <see cref="Supply{T}"/> so that it can be recycled.
        /// </summary>
        /// <param name="obj">The instance to return</param>
        /// <typeparam name="T">Type of returned instance</typeparam>
        /// <exception cref="Exception">Thrown if the object is not present in the used list, meaning that it was
        /// not <see cref="Supply{T}"/> </exception>
        public void Return<T>(T obj) where T : IManageableObject, new()
        {
            if (!_used[typeof(T)].Contains(obj))
                throw new Exception("cannot return an object that is not in use");

            _used[typeof(T)].Remove(obj);
            _pool[typeof(T)].Add(obj);
        }

        /// <summary>
        /// Return a previously <see cref="Supply"/> <see cref="GameObject"/> to the <see cref="ObjectManager"/> so that
        /// it can disabled it and use it later. 
        /// </summary>
        /// <param name="obj">The instance of a GameObject to be returned</param>
        /// <exception cref="Exception">Thrown if the object is not present in the used list, meaning that it was
        /// not <see cref="Supply"/></exception>
        public void Return(GameObject obj)
        {
            GameObject prefab = default;
            foreach ((GameObject key, List<GameObject> value) in _goUsed)
            {
                if (!value.Contains(obj)) continue;

                prefab = key;
                break;
            }

            if (prefab == default)
                throw new Exception("cannot return an object that is not in use");

            _goUsed[prefab].Remove(obj);
            _goPool[prefab].Add(obj);
            OrganizeReturnedInScene(obj);
        }

        void OrganizeReturnedInScene(GameObject obj)
        {
            string key = obj.name.Replace("(Clone)", "");

            Transform pool = _goPoolParent.transform.Find(key);

            if (pool == null)
            {
                GameObject container = new(key);
                container.transform.SetParent(_goPoolParent.transform);
                pool = container.transform;
            }

            obj.transform.SetParent(pool, false);
        }
    }
}
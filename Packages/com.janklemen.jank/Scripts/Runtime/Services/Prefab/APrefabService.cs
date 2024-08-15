using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jank.App;
using Jank.Objects;
using UnityEngine;

namespace Jank.Services.Prefab
{
    /// <summary>
    /// A scriptableObject that provides a way to define how to fetch prefabs using <see cref="TryGetPrefab"/>
    /// </summary>
    public abstract class APrefabService : AJankleScriptableObjectService, IPrefabService
    {
        public abstract IEnumerable<(string key, IPrefabService.PrefabListing listing)> Listings { get; }

        /// <inheritdoc />
        public abstract bool TryGetPrefab(string key, out GameObject prefab);

        public abstract GameObject ThrowOrGetPrefab(string key);
    }
    
    public abstract class APrefabService<T> : AJankleScriptableObjectService, IPrefabService<T>, IPrefabService, IRunOnContainerLoad 
    {
        /// <inheritdoc />
        public abstract bool TryGetPrefab(T key, out GameObject prefab);

        public abstract IEnumerable<(string key, IPrefabService.PrefabListing listing)> Listings { get; }
        public abstract bool TryGetPrefab(string key, out GameObject prefab);
        public abstract GameObject ThrowOrGetPrefab(string key);
        public abstract GameObject ThrowOrGetPrefab(T key);

        public async UniTask RunLoadOperation(ObjectManager manager)
        {
            foreach ((string key, IPrefabService.PrefabListing listing) in Listings)
            {
                for (int i = 0; i < listing.Prewarm; i++)
                    await manager.Prewarm(listing.Prefab);
            }
        }
    }
}
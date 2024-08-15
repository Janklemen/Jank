using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jank.Services.Prefab
{
    /// <summary>
    /// A service that provides a way to attempt to get prefabs for use in the game.
    /// </summary>
    public interface IPrefabService
    {
        [System.Serializable]
        public class PrefabListing
        {
            public GameObject Prefab;
            public int Prewarm;
        }
        
        IEnumerable<(string key, PrefabListing listing)> Listings { get; }
        
        /// <summary>
        /// Tries to get a prefab GameObject from the given key.
        /// </summary>
        /// <param name="key">The key to lookup the prefab.</param>
        /// <param name="prefab">The output parameter to store the fetched prefab GameObject, if found.</param>
        /// <returns>True if the prefab was found; otherwise, false.</returns>
        bool TryGetPrefab(string key, out GameObject prefab);

        GameObject ThrowOrGetPrefab(string key);
        
        public class PrefabDoesNotExistException : Exception
        {
            public PrefabDoesNotExistException() { }
            public PrefabDoesNotExistException(string message) : base(message)
            {
            }

            public PrefabDoesNotExistException(string message, Exception inner) : base(message, inner)
            {
            }
        }
    }

    public interface IPrefabService<T> : IPrefabService
    {
        bool TryGetPrefab(T key, out GameObject prefab);

        GameObject ThrowOrGetPrefab(T key);
    }
}
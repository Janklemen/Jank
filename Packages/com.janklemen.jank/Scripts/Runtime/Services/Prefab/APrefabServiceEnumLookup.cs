using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jank.Services.Prefab
{
    /// <summary>
    /// Lookup service for Prefab objects based on their GameObject names.
    /// </summary>
    public abstract class APrefabServiceEnumLookup<T> : APrefabService<T> 
        where T : struct
    {
        [System.Serializable]
        public struct Entry
        {
            public T Key;
            public IPrefabService.PrefabListing Value;
        }
        
        [SerializeField] List<Entry> Entries = new();

        public override IEnumerable<(string key, IPrefabService.PrefabListing listing)> Listings
        {
            get
            {
                List<(string, IPrefabService.PrefabListing)> list = new ();

                for (var i = 0; i < Entries.Count; i++)
                    list.Add((Entries[i].Key.ToString(), Entries[i].Value));

                return list;
            }
        }

        /// <inheritdoc />
        public override bool TryGetPrefab(T key, out GameObject prefab)
        {
            Entry entry = Entries.FirstOrDefault(entry => entry.Key.Equals(key));

            if (entry.Value == default)
            {
                prefab = null;
                return false;
            }

            prefab = entry.Value.Prefab;
            return true;
        }

        public override bool TryGetPrefab(string key, out GameObject prefab)
            => TryGetPrefab(Enum.Parse<T>(key), out prefab);

        public override GameObject ThrowOrGetPrefab(string key)
        {
            if (!TryGetPrefab(key, out GameObject prefab))
                throw new IPrefabService.PrefabDoesNotExistException($"key: {key}");

            return prefab;
        }

        public override GameObject ThrowOrGetPrefab(T key)
        {
            if (!TryGetPrefab(key, out GameObject prefab))
                throw new IPrefabService.PrefabDoesNotExistException($"key: {key.ToString()}");

            return prefab;
        }
    }
}
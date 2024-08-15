using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jank.Services.Prefab
{
    /// <summary>
    /// Lookup service for Prefab objects based on their GameObject names.
    /// </summary>
    [CreateAssetMenu(menuName = "Jank/Services/PrefabServiceObjectNameLookup")]
    public class PrefabServiceObjectNameLookup : APrefabService
    {
        [Tooltip("Prefabs in this list can be looked up via their name")]
        [SerializeField]
        List<IPrefabService.PrefabListing> _prefabs = new();

        public override IEnumerable<(string key, IPrefabService.PrefabListing listing)> Listings
            => _prefabs.Select(p => (p.Prefab.name, p)); 

        /// <inheritdoc />
        public override bool TryGetPrefab(string key, out GameObject prefab)
        {
            IPrefabService.PrefabListing listing = _prefabs.FirstOrDefault(o => o.Prefab.name == key);
            prefab = listing?.Prefab;
            return prefab != default;
        }

        public override GameObject ThrowOrGetPrefab(string key)
        {
            if (!TryGetPrefab(key, out GameObject prefab))
                throw new IPrefabService.PrefabDoesNotExistException($"key: {key}");

            return prefab;
        }
    }
}
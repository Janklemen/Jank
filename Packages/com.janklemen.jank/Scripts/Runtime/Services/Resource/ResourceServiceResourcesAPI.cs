using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Jank.Services.Resource
{
    /// <summary>
    /// Loads resources asynchronously using the <see cref="Resources"/> API.
    /// </summary>
    [CreateAssetMenu(menuName = "Jank/Services/ResourceAssetService")]
    public class ResourceServiceResourcesAPI : AResourceService
    {
        [SerializeField]
        public string SpritesResourcePath  = "Sprites";
        
        [SerializeField]
        public string Texture2DResourcePath = "Sprites";

        readonly Dictionary<string, Sprite> _spritesCache = new();
        readonly Dictionary<string, Texture2D> _textureCache = new();

        public override async UniTask<Sprite> GetSpriteAsset(string key) 
            => await GetResource(key, _spritesCache, SpritesResourcePath);

        public override async UniTask<Texture2D> GetTextureAsset(string key) 
            => await GetResource(key, _textureCache, Texture2DResourcePath);

        public async UniTask<T> GetResource<T>(string key, Dictionary<string, T> cache, string path) where T : Object
        {
            if (cache.TryGetValue(key, out T resource))
                return resource;

            string trueKey = Regex.Replace(key, @"\s+", string.Empty);

            Object loaded = await Resources.LoadAsync<T>(Path.Combine(path, trueKey));

            if (loaded == null)
            {
                Debug.LogError($"Could not find {typeof(T).Name} {trueKey}");
                return null;
            }

            T sp = loaded as T;
            cache[key] = sp;
            return sp;
        }
    }
}
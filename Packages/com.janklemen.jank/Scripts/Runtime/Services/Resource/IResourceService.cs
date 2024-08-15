using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jank.Services.Resource
{
    /// <summary>
    /// A service for looking up specific types of resources asynchronously. This should not be mixed up the the
    /// <see cref="Resources"/> API, which is a specific way to lookup resources.
    /// </summary>
    public interface IResourceService
    {
        /// <summary>
        /// Return a Sprite asset
        /// </summary>
        /// <param name="key">Key associated with this asset</param>
        /// <returns>Sprite, or null if lookup failed</returns>
        public UniTask<Sprite> GetSpriteAsset(string key);
        
        /// <summary>
        /// Return a Texture2D asset
        /// </summary>
        /// <param name="key">Key associated with this asset</param>
        /// <returns>Texture2D, or null if lookup failed</returns>
        public UniTask<Texture2D> GetTextureAsset(string key);
    }
}
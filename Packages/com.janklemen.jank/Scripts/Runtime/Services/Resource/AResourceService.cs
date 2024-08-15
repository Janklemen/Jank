using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Jank.Services.Resource
{
    /// <summary>
    /// A <see cref="ScriptableObject"/> that allows configuration of lookup for resources.
    /// </summary>
    public abstract class AResourceService : AJankleScriptableObjectService, IResourceService
    {
        /// <inheritdoc />
        public abstract UniTask<Sprite> GetSpriteAsset(string key);

        /// <inheritdoc />
        public abstract UniTask<Texture2D> GetTextureAsset(string key);
    }
}
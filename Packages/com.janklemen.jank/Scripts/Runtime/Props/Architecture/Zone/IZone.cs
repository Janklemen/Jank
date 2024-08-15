using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jank.Props.Architecture.Token;

namespace Jank.Props.Architecture.Zone
{
    /// <summary>
    /// Zones accept pieces
    /// </summary>
    public interface IZone : ILoadable, IInteractable, ITransformable
    {
        IReadOnlyList<IToken> Tokens { get; }
        
        /// <summary>
        /// A piece is added to the zone. Pieces perform the logic of moving to the zone.
        /// </summary>
        UniTask Accept(IToken token, int order = 0);

        /// <summary>
        /// A piece is removed from the zone. Pieces perform the logic of visualizing this release.
        /// </summary>
        UniTask Release(IToken token);

        /// <summary>
        /// Get the current order of the token, or -1 if the token does not exist on the zone
        /// </summary>
        int GetTokenOrder(IToken token);

        /// <summary>
        /// Used to tell the zone that some order information may have changed with the tokens, and therefore
        /// the tokens must be reordered. 
        /// </summary>
        /// <returns></returns>
        UniTask Reorder();
    }

    public interface IZone<TData> : IZone, ILoadable<TData> { }
}
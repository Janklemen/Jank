using Cysharp.Threading.Tasks;
using Jank.Props.Architecture.Zone;

namespace Jank.Props.Architecture.Token
{
    /// <summary>
    /// A piece is something that is moved around the game between sections
    /// </summary>
    public interface IToken : ILoadable, IInteractable, ITransformable
    {
        IZone CurrentZone { get; }
        
        /// <summary>
        /// Move the piece to the provided target. It should parent itself to the new target then perform what ever
        /// movement is required to get it there.
        /// </summary>
        UniTask MoveTo(IZone zone, int order = 0);
        
        /// <summary>
        /// Limbo is the place pieces live when they're not attached to any zone.
        /// remove a piece.
        /// </summary>
        UniTask MoveToLimbo();
    }
}
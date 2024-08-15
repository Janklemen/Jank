using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jank.Enums;

namespace Jank.Props.Architecture
{
    public interface IInteractable
    {
        EInteractionStatus InteractionStatus { get; }
        
        UniTask SetInteractable(bool isInteractable);
    }

    public static class UTIInteractable
    {
        public static async UniTask SetInteractableAll(this IEnumerable<IInteractable> interactables, bool isInteractable)
        {
            foreach (IInteractable interactable in interactables)
                await interactable.SetInteractable(isInteractable);
        }
    }
}
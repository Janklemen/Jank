using UnityEngine;

namespace Jank.Utilities
{
    public interface IComponentReferencer
    {
        Component ComponentReference { get; }
    }
}
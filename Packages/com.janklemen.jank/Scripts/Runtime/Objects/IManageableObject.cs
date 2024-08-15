using Cysharp.Threading.Tasks;

namespace Jank.Objects
{
    /// <summary>
    /// An object that can be recycled by an <see cref="ObjectManager"/>
    /// </summary>
    public interface IManageableObject
    {
        /// <summary>
        /// Remove all instance specific data and set it to default to that instance is blank when recycling is
        /// performed.
        /// </summary>
        UniTask Clear();
    }
}
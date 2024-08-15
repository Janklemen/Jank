using System.Collections.Generic;

namespace Jank.DataStructures.Graph
{
    /// <summary>
    /// Simple Tree nodes have a value and chidren
    /// The value held by the tree is of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleGraphNode<T>
    {
        /// <summary>
        /// Value of the node
        /// </summary>
        public T Value;

        /// <summary>
        /// Children of the node
        /// </summary>
        public List<SimpleGraphNode<T>> Children = new();

        /// <summary>
        /// Create a node with value T
        /// </summary>
        /// <param name="value"></param>
        public SimpleGraphNode(T value)
        {
            Value = value;
        }
    }
}

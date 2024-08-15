using System;

namespace Jank.Serialization
{
    /// <summary>
    /// Marks classes for processing by the JankSerializableGenerator which generates a <see cref="IJankSerializable"/>
    /// conterpart. It will automatically generate deserialization code for any fields that have the [SerializeField]
    /// attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JankSerializableAttribute : Attribute
    {
    }
}

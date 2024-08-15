namespace Jank.Serialization
{
    /// <summary>
    /// Implementation define serialization and deserialization of an object.
    /// </summary>
    /// <remarks>
    /// <para>The intended place for this interface to exist is as a source generated partial class
    /// created by the JankSerializableGenerator. This generator creates implementations of the
    /// <see cref="Serialize"/> and <see cref="Deserialize"/> functions.</para>
    /// </remarks> 
    public interface IJankSerializable
    {
        /// <summary>
        /// Serializes the object into a string representation.
        /// </summary>
        /// <returns>A string representation of the serialized object.</returns>
        string Serialize();

        /// <summary>
        /// Deserializes the specified data string.
        /// </summary>
        /// <param name="data">The string representation of the serialized data.</param>
        void Deserialize(string data);
    }
}
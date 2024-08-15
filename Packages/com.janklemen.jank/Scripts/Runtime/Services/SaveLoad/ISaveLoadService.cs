using Cysharp.Threading.Tasks;
using Jank.Serialization;

namespace Jank.Services.SaveLoad
{
    /// <summary>
    /// Service for managing saved game data and loading it back
    /// </summary>
    /// <remarks>
    /// This service uses a slot approach, where a number of slots are assumed, then metadata + save data are saved to
    /// a slot. This makes it easy to look up and paginate for save/load systems.
    /// </remarks>
    public interface ISaveLoadService
    {
        /// <summary>
        /// Retrieves the metadata for the given slots and data.
        /// </summary>
        /// <param name="slots">An array of indices representing the metadata to retrieve.</param>
        /// <param name="data">An array of objects implementing the IJankSerializable interface.</param>
        /// <returns>A UniTask representing the asynchronous operation to retrieve the metadata.</returns>
        UniTask GetMetaData(int[] slots, IJankSerializable[] data);


        /// <summary>
        /// Loads data from a specific slot into the provided serializable object.
        /// </summary>
        /// <param name="slot">The slot number from which to load the data.</param>
        /// <param name="loadInto">The serializable object into which the data will be loaded.</param>
        /// <returns>
        /// An awaitable UniTask that represents the asynchronous loading operation.
        /// </returns>
        UniTask LoadFromSlot(int slot, IJankSerializable loadInto);


        /// <summary>
        /// Saves the data and metadata to the specified slot using the Jank serialization format.
        /// </summary>
        /// <param name="data">The data to be saved.</param>
        /// <param name="metaData">The metadata to be saved.</param>
        /// <param name="slot">The slot number where the data and metadata will be saved.</param>
        /// <returns>A UniTask representing the asynchronous save operation.</returns>
        UniTask SaveToSlot(IJankSerializable data, IJankSerializable metaData, int slot);
    }
}
using Cysharp.Threading.Tasks;
using Jank.Serialization;

namespace Jank.Services.SaveLoad
{
    /// <summary>
    /// A ScriptableObject that can be used to configure the behaviour of save and load operations.
    /// </summary>
    public abstract class ASaveLoadService : AJankleScriptableObjectService, ISaveLoadService 
    {
        /// <inheritdoc />
        public abstract UniTask GetMetaData(int[] slots, IJankSerializable[] data);

        /// <inheritdoc />
        public abstract UniTask LoadFromSlot(int slot, IJankSerializable loadInto);

        /// <inheritdoc />
        public abstract UniTask SaveToSlot(IJankSerializable data, IJankSerializable metaData, int slot);
    }
}
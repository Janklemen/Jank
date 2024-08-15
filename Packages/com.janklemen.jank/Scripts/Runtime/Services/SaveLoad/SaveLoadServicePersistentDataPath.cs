using System;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Jank.Serialization;
using UnityEngine;
using Application = UnityEngine.Application;

namespace Jank.Services.SaveLoad
{
    /// <summary>
    /// A <see cref="ISaveLoadService"/> that uses Application.persistentDataPath to determine where to save data
    /// </summary>
    [CreateAssetMenu(menuName = "Jank/Services/SaveLoadService")]
    public class SaveLoadServicePersistentDataPath : ASaveLoadService 
    {
        const string cSaveDirectory = "saves";
        const string cMetadataExtension = "metadata";
        const string cSaveExtension = "save";
        
        string _path;

        /// <inheritdoc />
        public override void InitializeService()
        {
            _path = Application.persistentDataPath;
        }

        /// <inheritdoc />
        public override async UniTask GetMetaData(int[] slots, IJankSerializable[] data)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                await DeserializeObject(slots[i], data[i], cMetadataExtension);
            }
        }

        /// <inheritdoc />
        public override async UniTask LoadFromSlot(int slot, IJankSerializable loadInto)
        {
            await DeserializeObject(slot, loadInto, cSaveExtension);
        }

        /// <inheritdoc />
        public override async UniTask SaveToSlot(IJankSerializable data, IJankSerializable metaData, int slot)
        {
            FileInfo fi = new(ConstructPath(slot, cSaveExtension));

            if (fi.Directory != null && !fi.Directory.Exists)
                fi.Directory.Create();
            
            await File.WriteAllTextAsync(ConstructPath(slot, cSaveExtension), data.Serialize());
            await File.WriteAllTextAsync(ConstructPath(slot, cMetadataExtension), metaData.Serialize());
            
            Debug.Log($"Saved game to {fi.Directory}");
        }
        
        async Task DeserializeObject(int slot, IJankSerializable loadInto, string extension)
        {
            string path = ConstructPath(slot, extension);

            if (!File.Exists(path))
                return;

            string data = await File.ReadAllTextAsync(path);

            try
            { 
                loadInto.Deserialize(data);
            }
            catch (Exception)
            {
                Debug.LogWarning($"Failed to debug the file {path}. It is probably corrupt. Ignoring file.");
            }
        }

        string ConstructPath(int slot, string extension)
        {
            return Path.Combine(_path, cSaveDirectory, $"slot{slot}.{extension}");
        }
    }
}
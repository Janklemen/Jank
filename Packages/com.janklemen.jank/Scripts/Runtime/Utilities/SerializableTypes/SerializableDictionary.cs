using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jank.Utilities.SerializableTypes
{
    public abstract class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] List<TKey> _keys = new();
        [SerializeField] List<TValue> _values = new();

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                _keys.Add(_keys.Contains(pair.Key) ? ProcessRepeatKey(pair.Key) : pair.Key);
                _values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < _keys.Count; i++)
                Add(
                    ContainsKey(_keys[i]) ? ProcessRepeatKey(_keys[i]) : _keys[i],
                    _values.Count > i ? _values[i] : default
                );
        }

        public abstract TKey ProcessRepeatKey(TKey key);
    }

    [Serializable]
    public class SerilizableStringDictionary<TValue> : SerializableDictionary<string, TValue>
    {
        public override string ProcessRepeatKey(string key)
        {
            return key + "_next";
        }
    }
}


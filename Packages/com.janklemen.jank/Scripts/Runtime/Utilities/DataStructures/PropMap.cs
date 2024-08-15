using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jank.Objects;
using Jank.Props.Architecture;
using UnityEngine;

namespace Jank.Utilities.DataStructures
{
    public interface IPropKey<TPropData>
    {
        public TPropData PropLoadData { get; }
    }

    public class PropMap<TPropLoadData, TProp> : Dictionary<IPropKey<TPropLoadData>, TProp>
        where TProp : AProp
    {
        readonly IObjectPool _pool;
        readonly GameObject _prefab;
        readonly ActionAsync<TPropLoadData, TProp> _processor;

        public PropMap(IObjectPool objectPool, GameObject prefab, ActionAsync<TPropLoadData, TProp> processor)
        {
            _pool = objectPool;
            _prefab = prefab;
            _processor = processor;
        }

        public new void Clear()
        {
            _pool.ReturnMany(Values);
            base.Clear();
        }

        public async UniTask<TProp> AddFromData(IPropKey<TPropLoadData> key)
        {
            await RemoveProp(key);

            GameObject instance = await _pool.Supply(_prefab);
            instance.transform.SetParent(null);
            TProp prop = instance.GetComponent<TProp>();

            TPropLoadData loadData = default;

            loadData = key.PropLoadData;
            await _processor(loadData, prop);

            prop.DebugData = key;
            this[key] = prop;
            return prop;
        }

        public async UniTask<TProp> AddFromData(TPropLoadData data)
            => await AddFromData(new BasicKey(data));

        public async UniTask<TProp> AddFromData<TGroupKey>(TPropLoadData data, TGroupKey group)
            => await AddFromData(new GroupKey<TGroupKey>(data, group));

        public async UniTask AddFromDatas(IEnumerable<IPropKey<TPropLoadData>> keys)
        {
            foreach (IPropKey<TPropLoadData> state in keys)
                await AddFromData(state);
        }

        public async UniTask AddFromDatas(IEnumerable<TPropLoadData> keys)
        {
            foreach (TPropLoadData state in keys)
                await AddFromData(state);
        }

        public async UniTask AddFromDatas<TGroupKey>(IEnumerable<TPropLoadData> keys, TGroupKey group)
        {
            foreach (TPropLoadData state in keys)
                await AddFromData(state, group);
        }

        public async UniTask RemoveProp(IPropKey<TPropLoadData> key)
        {
            if (TryGetValue(key, out TProp prop))
            {
                await prop.Unload();
                _pool.Return(prop.gameObject);
                Remove(key);
            }
        }

        public async UniTask RemoveProp(TPropLoadData key)
            => await RemoveProp(new BasicKey(key));

        public async UniTask RemoveProp<TGroupKey>(TPropLoadData key, TGroupKey group)
            => await RemoveProp(new GroupKey<TGroupKey>(key, group));

        public async UniTask RemoveProps(IEnumerable<IPropKey<TPropLoadData>> keys)
        {
            foreach (IPropKey<TPropLoadData> key in keys)
            {
                await RemoveProp(key);
            }
        }

        public async UniTask RemoveProps(IEnumerable<TPropLoadData> keys)
        {
            foreach (TPropLoadData key in keys)
                await RemoveProp(key);
        }

        public async UniTask RemoveProps<TGroupKey>(IEnumerable<TPropLoadData> keys, TGroupKey group)
        {
            foreach (TPropLoadData key in keys)
                await RemoveProp(key, group);
        }

        public async UniTask<IEnumerable<TProp>> GetProps(IEnumerable<IPropKey<TPropLoadData>> keys)
        {
            List<TProp> props = new();

            foreach (IPropKey<TPropLoadData> key in keys)
                props.Add(await GetProp(key));

            return props;
        }

        public async UniTask<IEnumerable<TProp>> GetProps(IEnumerable<TPropLoadData> keys)
        {
            List<TProp> props = new();

            foreach (TPropLoadData key in keys)
                props.Add(await GetProp(key));

            return props;
        }

        public async UniTask<IEnumerable<TProp>> GetProps<TGroupKey>(IEnumerable<TPropLoadData> keys, TGroupKey group)
        {
            List<TProp> props = new();

            foreach (TPropLoadData key in keys)
                props.Add(await GetProp(key, group));

            return props;
        }

        public async UniTask<TProp> GetProp(IPropKey<TPropLoadData> key)
        {
            if (!TryGetValue(key, out TProp prop))
                prop = await AddFromData(key);

            return prop;
        }

        public async UniTask<TProp> GetProp(TPropLoadData data)
            => await GetProp(new BasicKey(data));

        public async UniTask<TProp> GetProp<TGroupKey>(TPropLoadData data, TGroupKey group)
            => await GetProp(new GroupKey<TGroupKey>(data, group));

        public bool ContainsProp(IPropKey<TPropLoadData> key) => ContainsKey(key);
        public bool ContainsProp(TPropLoadData data) => ContainsProp(new BasicKey(data));
        public bool ContainsProp<TGroupKey>(TPropLoadData data, TGroupKey group) 
            => ContainsProp(new GroupKey<TGroupKey>(data, group));


        public struct BasicKey : IPropKey<TPropLoadData>
        {
            readonly TPropLoadData _propLoadData;

            public BasicKey(TPropLoadData propLoadData)
            {
                _propLoadData = propLoadData;
            }

            public TPropLoadData PropLoadData => _propLoadData;
        }

        public struct GroupKey<TCombinedData> : IPropKey<TPropLoadData>
        {
            readonly TPropLoadData _propLoadData;
            readonly TCombinedData _group;

            public GroupKey(TPropLoadData propLoadData, TCombinedData group)
            {
                _propLoadData = propLoadData;
                _group = group;
            }

            public TPropLoadData PropLoadData => _propLoadData;
        }
    }
}
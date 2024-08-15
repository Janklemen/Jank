using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Jank.Objects
{
    public static class UTObjectPool
    {
        public static void ReturnMany(this IObjectPool pool, IEnumerable<MonoBehaviour> objects)
        {
            foreach (MonoBehaviour mono in objects)
                pool.Return(mono.gameObject);
        }

        public static async UniTask<T> Supply<T>(this IObjectPool pool, GameObject prefab)
            where T : MonoBehaviour
        {
            GameObject instance = await pool.Supply(prefab);
            T monoBehaviour = instance.GetComponent<T>();

            if (monoBehaviour == null)
                throw new Exception($"Provided prefab was invalid. It does not contain the component {typeof(T).Name}");

            return monoBehaviour;
        }
    }
}
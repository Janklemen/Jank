using UnityEngine;

namespace Jank.Utilities
{
    public class LazyComponent<TComponent>
    {
        TComponent _cached;
        
        public TComponent Get(GameObject parent)
        {
            if (_cached == null)
                _cached = parent.GetComponent<TComponent>();

            if (_cached == null)
            {
                Debug.LogError($"Fail LazyLoad component. Component of type {typeof(TComponent).Name} is not present on gameobject {parent.name}");
            }

            return _cached;
        }
    }
}
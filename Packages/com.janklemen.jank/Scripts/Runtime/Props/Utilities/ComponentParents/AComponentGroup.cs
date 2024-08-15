using System.Collections.Generic;
using Jank.Utilities;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Jank.Props.Utilities.ComponentParents
{
    /// <summary>
    /// Collects all components of a certain type in the hierarchy. Makes it easy to do actions over
    /// all components, like disabling all renderers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExecuteInEditMode]
    public abstract class AComponentGroup<T> : MonoBehaviour
        where T : Component
    {
        [SerializeField] List<T> _components = new();

        public IReadOnlyList<T> Components
        {
            get
            {
                PopulateComponentsList();
                return _components;
            }
        }

#if UNITY_EDITOR
        void OnEnable() => EditorApplication.hierarchyChanged += PopulateComponentsList;
        void OnDisable() => EditorApplication.hierarchyChanged -= PopulateComponentsList;
#endif
        
        public void PopulateComponentsList()
        {
            _components.Clear();
            
            transform.ProcessHierarchy(t =>
            {
                if(t.CompareTag(TagHandle.GetExistingTag("EditorOnly")))
                    return;
                
                T comp = t.GetComponent<T>();
                
                if(comp != null)
                    _components.Add(comp);
            });
        }
    }
}
#if UNITY_EDITOR
using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

namespace Jank.Inspector.InspectionHandlers
{
    public class UnityEngineObjectInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            return type.IsSubclassOf(typeof(UnityEngine.Object));
        }

        public VisualElement RenderInspector(string name, object value)
        {
            ObjectField element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            ObjectField element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        ObjectField CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case UnityEngine.Object obj:
                    return new ObjectField(name) { objectType = value.GetType(), allowSceneObjects = true, value = obj };
                default:
                    return new ObjectField(name) { value = default };
            }
        }
    }
}
#endif
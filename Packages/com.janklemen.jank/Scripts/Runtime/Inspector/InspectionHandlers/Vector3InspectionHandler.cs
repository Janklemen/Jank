#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class Vector3InspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            if (type == typeof(Vector3))
                return true;

            return false;
        }

        public VisualElement RenderInspector(string name, object value)
        {
            Vector3Field element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            Vector3Field element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        Vector3Field CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case Vector3 vec3value:
                    return new Vector3Field(name) { value = vec3value };
                default:
                    return new Vector3Field(name) { value = default };
            }
        }
    }
}
#endif
#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class Vector4InspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            if (type == typeof(Vector4))
                return true;

            return false;
        }

        public VisualElement RenderInspector(string name, object value)
        {
            Vector4Field element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            Vector4Field element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        Vector4Field CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case Vector4 vec4value:
                    return new Vector4Field(name) { value = vec4value };
                default:
                    return new Vector4Field(name) { value = default };
            }
        }
    }
}
#endif
#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class Vector2InspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            if (type == typeof(Vector2))
                return true;

            return false;
        }

        public VisualElement RenderInspector(string name, object value)
        {
            Vector2Field element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            Vector2Field element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        Vector2Field CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case Vector2 vec2value:
                    return new Vector2Field(name) { value = vec2value };
                default:
                    return new Vector2Field(name) { value = default };
            }
        }
    }
}
#endif
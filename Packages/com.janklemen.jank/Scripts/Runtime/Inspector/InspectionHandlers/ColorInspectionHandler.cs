#if UNITY_EDITOR
using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class ColorInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            return type == typeof(Color);
        }

        public VisualElement RenderInspector(string name, object value)
        {
            ColorField element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            ColorField element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        ColorField CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case Color colorValue:
                    return new ColorField(name) { value = colorValue };
                default:
                    return new ColorField(name) { value = Color.white };
            }
        }
    }
}
#endif
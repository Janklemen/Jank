#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class RectInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            return type == typeof(Rect);
        }

        public VisualElement RenderInspector(string name, object value)
        {
            RectField element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            RectField element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        RectField CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case Rect rectValue:
                    return new RectField(name) { value = rectValue };
                default:
                    return new RectField(name) { value = default };
            }
        }
    }
}
#endif
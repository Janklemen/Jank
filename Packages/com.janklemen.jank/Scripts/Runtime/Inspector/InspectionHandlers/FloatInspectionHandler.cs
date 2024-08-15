#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class FloatInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            if (type == typeof(float) || type == typeof(double))
                return true;

            return false;
        }

        public VisualElement RenderInspector(string name, object value)
        {
            FloatField element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            FloatField element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        FloatField CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case float fvalue:
                    return new FloatField(name) { value = fvalue };
                case double dvalue:
                    return new FloatField(name) { value = (float)dvalue };
                default:
                    return new FloatField(name) { value = 0.0f };
            }
        }
    }
}
#endif
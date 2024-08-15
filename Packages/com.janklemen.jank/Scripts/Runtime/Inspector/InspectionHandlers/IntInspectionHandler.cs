#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class IntInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            if (type == typeof(byte) || type == typeof(short) || type == typeof(int) || type == typeof(long))
                return true;

            return false;
        }

        public VisualElement RenderInspector(string name, object value)
        {
            IntegerField element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            IntegerField element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        IntegerField CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case byte bvalue:
                    return new IntegerField(name) { value = bvalue };
                case short svalue:
                    return new IntegerField(name) { value = svalue };
                case int ivalue:
                    return new IntegerField(name) { value = ivalue };
                case long lvalue:
                    return new IntegerField(name) { value = (int)lvalue };
                default:
                    return new IntegerField(name) { value = 0 };
            }
        }
    }
}
#endif
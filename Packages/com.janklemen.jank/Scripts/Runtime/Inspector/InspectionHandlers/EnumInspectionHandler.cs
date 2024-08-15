#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class EnumInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            if (type.IsEnum)
                return true;

            return false;
        }

        public VisualElement RenderInspector(string name, object value)
        {
            EnumField element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            EnumField element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        EnumField CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case Enum enumValue:
                    return new EnumField(name, (Enum)value) { value = enumValue };
                default:
                    return new EnumField(name, (Enum)Enum.GetValues(value.GetType()).GetValue(0));
            }
        }
    }
}
#endif
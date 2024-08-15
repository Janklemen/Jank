#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class BoolInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            if (type == typeof(bool))
                return true;

            return false;
        }

        public VisualElement RenderInspector(string name, object value)
        {
            Toggle element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            Toggle element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        Toggle CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case bool bvalue:
                    return new Toggle(name) { value = bvalue };
                default:
                    return new Toggle(name) { value = false };
            }
        }
    }
}
#endif
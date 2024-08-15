#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class CharInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            if (type == typeof(char))
                return true;

            return false;
        }

        public VisualElement RenderInspector(string name, object value)
        {
            TextField element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            TextField element = CreateFromValue(name, defaultValue);
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        TextField CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case char cvalue:
                    return new TextField(name) { value = cvalue.ToString() };
                default:
                    return new TextField(name) { value = string.Empty };
            }
        }
    }
}
#endif
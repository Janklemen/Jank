#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class TextInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type) => Type.GetTypeCode(type) == TypeCode.String;

        public VisualElement RenderInspector(string name, object value)
        {
            TextField element = new(name) { value = value as string };
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            TextField element = new(name) { value = defaultValue as string };
            getter = () => element.value;
            UTInspection.PrepareInspectorInput(element);
            return element;
        }
    }
}
#endif
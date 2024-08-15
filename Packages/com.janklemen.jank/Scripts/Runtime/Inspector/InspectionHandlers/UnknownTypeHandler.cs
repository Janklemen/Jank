#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class UnknownTypeHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type) => true;

        public VisualElement RenderInspector(string name, object value)
        {
            Label element = new Label($"{name} (Type Unknown) = {value}");
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            Label element = new($"{name} (Type Unknown) = {defaultValue}");
            getter = () => defaultValue;
            element.SetEnabled(false);
            return element;
        }
    }
    
    public class SystemObjectInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type) => true;

        public VisualElement RenderInspector(string name, object value)
        {
            ObjectInspector inspector = new ObjectInspector(name, value);
            return inspector;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            Label element = new($"INPUT: {name} (Type Unknown) = {defaultValue}");
            getter = () => defaultValue;
            element.SetEnabled(false);
            return element;
        }
    }
}
#endif
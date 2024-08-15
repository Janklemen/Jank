#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public interface ITypeInspectionHandler
    {
        public bool HandlesType(Type type);

        public VisualElement RenderInspector(string name, object value);

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default);
    }
}
#endif
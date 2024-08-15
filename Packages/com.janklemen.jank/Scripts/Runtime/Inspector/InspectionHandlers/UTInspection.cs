#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public static class UTInspection
    {
        static ITypeInspectionHandler[] _handlers =
        {
            new TextInspectionHandler(),
            new IntInspectionHandler(),
            new FloatInspectionHandler(),
            new BoolInspectionHandler(),
            new CharInspectionHandler(),
            new Vector2InspectionHandler(),
            new Vector3InspectionHandler(),
            new Vector4InspectionHandler(),
            new QuaternionInspectionHandler(),
            new ColorInspectionHandler(),
            new RectInspectionHandler(),
            new EnumInspectionHandler(),
            new UnityEngineObjectInspectionHandler(),
            new SystemObjectInspectionHandler()
        };

        static ITypeInspectionHandler _unknownTypeHandler = new UnknownTypeHandler();

        public static ITypeInspectionHandler GetHandler(Type t)
        {
            foreach (ITypeInspectionHandler handler in _handlers)
            {
                if (handler.HandlesType(t))
                    return handler;
            }

            return _unknownTypeHandler;
        }

        public static void PrepareInspectorElement(VisualElement element)
        {
            element.SetEnabled(false);
            element.AddToClassList("unity-base-field__aligned");
        }

        public static void PrepareInspectorInput(VisualElement element)
        {
            element.SetEnabled(true);
            element.AddToClassList("unity-base-field__aligned");
        }
    }
}
#endif
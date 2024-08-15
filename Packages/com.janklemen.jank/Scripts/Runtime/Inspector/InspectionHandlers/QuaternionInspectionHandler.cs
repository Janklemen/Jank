#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector.InspectionHandlers
{
    public class QuaternionInspectionHandler : ITypeInspectionHandler
    {
        public bool HandlesType(Type type)
        {
            if (type == typeof(Quaternion))
                return true;

            return false;
        }

        public VisualElement RenderInspector(string name, object value)
        {
            Vector3Field element = CreateFromValue(name, value);
            UTInspection.PrepareInspectorElement(element);
            return element;
        }

        public VisualElement RenderInput(string name, out DGetter getter, object defaultValue = default)
        {
            Vector3Field element = CreateFromValue(name, defaultValue);
            getter = () => Quaternion.Euler(element.value); // return Quaternion from Euler angles
            UTInspection.PrepareInspectorInput(element);
            return element;
        }

        Vector3Field CreateFromValue(string name, object value)
        {
            switch (value)
            {
                case Quaternion quaternionValue:
                    return new Vector3Field(name) { value = quaternionValue.eulerAngles }; // Quaternion to Euler angles
                default:
                    return new Vector3Field(name) { value = default };
            }
        }
    }
}
#endif
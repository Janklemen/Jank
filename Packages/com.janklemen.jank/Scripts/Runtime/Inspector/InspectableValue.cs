#if UNITY_EDITOR
using System;
using Jank.Inspector.InspectionHandlers;
using UnityEngine.UIElements;

namespace Jank.Inspector
{
    /// <summary>
    /// A Generic Element that handles the inspection of anything
    /// </summary>
    public class InspectableValue : VisualElement
    {
        DGetter ObjectGetter = null;
        public object Value => ObjectGetter?.Invoke();

        public InspectableValue(string name, Type type, bool enabled = false)
        {
            SetData(name, type, null, enabled);
        }

        public InspectableValue(string name, Type type, object value, bool enabled = false)
        {
            SetData(name, type, value, enabled);
        }

        public void SetData(string name, Type type, object value, bool enabled = false)
        {
            VisualElement valToAdd = null;

            if (enabled)
            {
                Add(UTInspection.GetHandler(type).RenderInput(name, out ObjectGetter, value));
            }
            else
            {
                Add(UTInspection.GetHandler(type).RenderInspector(name, value));
            }
            
            
            return;
            
            
            switch (Type.GetTypeCode(type))
            {
                default:
                    {
                        valToAdd = new ObjectInspector(name, value);
                    }
                    break;
            }

            if (valToAdd != null)
            {
                valToAdd.AddToClassList("unity-base-field__aligned");
                valToAdd.SetEnabled(enabled);
                Add(valToAdd);
            }
        }
    }
}
#endif
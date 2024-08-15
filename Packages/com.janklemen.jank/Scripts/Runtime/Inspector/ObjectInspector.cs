#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Jank.Inspector.CustomEditorGenerator;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector
{
    public class ObjectInspector : Foldout
    {
        public ObjectInspector(string memberName, object instance)
        {
            Clear();
            
            if (instance == null)
                return;

            text = memberName;
            
            MemberInfo[] members =
                instance.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (MemberInfo memberInfo in members)
            {
                foreach (Attribute attribute in memberInfo.GetCustomAttributes())
                {
                    switch (attribute)
                    {
                        case JankSpaceAttribute space:
                            Add(new VisualElement() {style = { height = space.Space}} );
                            break;
                        case JankHeaderAttribute header:
                            Add(new Label(header.Header) { style = { unityFontStyleAndWeight = FontStyle.Bold} });
                            break;
                    }
                }
                
                if (memberInfo is FieldInfo field)
                {
                    object fieldValue = field.GetValue(instance);
                    
                    if(fieldValue != null && fieldValue.Equals(instance))
                        continue;
                    
                    DrawValue(field.Name, fieldValue);
                }

                if (memberInfo is PropertyInfo property)
                {
                    object propertyValue = property.GetValue(instance);
                    
                    if(propertyValue != null && propertyValue.Equals(instance))
                        continue;
                    
                    DrawValue(property.Name, propertyValue);
                }
            }
        }
        
        void DrawValue(string memberName, object memberValue)
        {
            if (memberName.Contains("k__BackingField"))
                return;

            if (memberValue == null)
            {
                UTCustomEditor.DrawNullObject(this, memberName);
                return;
            }
            
            Add(new InspectableValue(memberName, memberValue.GetType(), memberValue));
        }
    }
}
#endif
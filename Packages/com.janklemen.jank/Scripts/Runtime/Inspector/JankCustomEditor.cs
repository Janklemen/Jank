#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using Jank.Inspector.CustomEditorGenerator;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector
{
    public class JankCustomEditor : VisualElement
    {
        public JankCustomEditor(object instance, SerializedObject serializedObject, Type type)
        {
            SetData(instance, serializedObject, type);
        }

        public void SetData(object instance, SerializedObject serializedObject, Type type)
        {
            MonoScript[] scripts = Resources.FindObjectsOfTypeAll<MonoScript>();

            string classHeader = type.Name.Contains("`1")
                ? type.Name.Replace("`1", "") + $"<{type.GetGenericArguments()[0].Name}>"
                : type.Name;

            string monoScriptName = type.Name.Contains("`")
                ? type.Name.Substring(0, type.Name.IndexOf("`"))
                : type.Name;

            Label title = new(classHeader);
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            Add(title);

            MonoScript script = scripts.FirstOrDefault(s => s.name == monoScriptName);

            if (script != null)
            {
                ObjectField obj = new("Script")
                {
                    objectType = typeof(MonoScript),
                    value = script,
                    allowSceneObjects = false
                };
                obj.SetEnabled(false);
                obj.AddToClassList("unity-base-field__aligned");
                Add(obj);
            }
            
            var members = type.GetMembers(
                BindingFlags.Static | 
                BindingFlags.Instance | 
                BindingFlags.Public | 
                BindingFlags.NonPublic).OrderBy(m => m.MetadataToken);

            foreach (MemberInfo member in members)
            {
                if(member.Name.Contains("__backingObject"))
                    continue;
                
                foreach (Attribute attribute in member.GetCustomAttributes())
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

                if (UTMemberHandler.TryGetMemberHandler(member, out IMemberHandler handler))
                {
                    if(member is FieldInfo fi)
                        handler.Render(this, instance, serializedObject, fi.FieldType, member);
                    
                    if(member is PropertyInfo pi)
                        handler.Render(this, instance, serializedObject, pi.PropertyType, member);
                    
                    if(member is MethodInfo mi)
                        handler.Render(this, instance, serializedObject, null, member);
                }
            }
        }
    }
}
#endif
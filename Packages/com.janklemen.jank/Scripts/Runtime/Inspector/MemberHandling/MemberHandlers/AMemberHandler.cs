#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector.CustomEditorGenerator
{
    public abstract class AMemberHandler : IMemberHandler
    {
        public abstract bool CanHandleMember(MemberInfo member);
        public abstract int ProvideMemberHash(MemberInfo memberInfo);
        public abstract string Generate(MemberInfo info, IMemberHandler.GenerationEvent evt);
        public abstract void Render(VisualElement root, object instance, SerializedObject serializedObject, Type type, MemberInfo member);

        public string GetDecoratorString(MemberInfo member)
        {
            IEnumerable<Attribute> attributes = member.GetCustomAttributes();

            StringBuilder sb = new();
            
            foreach (Attribute attribute in attributes)
            {
                switch (attribute)
                {
                    case JankSpaceAttribute space:
                        sb.AppendLine($"root.Add(new VisualElement() {{style = {{ height = {space.Space} }}}} );");
                        break;
                    case JankHeaderAttribute header:
                        sb.AppendLine($"root.Add(new Label(\"{header.Header}\") {{ style = {{ unityFontStyleAndWeight = FontStyle.Bold}} }});");
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
#endif
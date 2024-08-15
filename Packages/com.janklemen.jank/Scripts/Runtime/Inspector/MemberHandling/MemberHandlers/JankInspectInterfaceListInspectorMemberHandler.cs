#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using UnityEditor;
using UnityEngine.UIElements;

namespace Jank.Inspector.CustomEditorGenerator
{
    public class JankInspectInterfaceListInspectorMemberHandler : AMemberHandler
    {
        public override bool CanHandleMember(MemberInfo member)
        {
            if (!member.GetCustomAttributes().Any(a => a is JankInspectAttribute))
                return false;

            if (member is FieldInfo fi && fi.FieldType.IsGenericType && 
                typeof(IList).IsAssignableFrom(fi.FieldType.GetGenericTypeDefinition()))
            {
                Type listType = fi.FieldType.GetGenericArguments()[0];
                if (listType.IsInterface)
                    return true;
            }

            if (member is PropertyInfo pi && pi.PropertyType.IsGenericType && 
                typeof(IList).IsAssignableFrom(pi.PropertyType.GetGenericTypeDefinition())) 
            {
                Type listType = pi.PropertyType.GetGenericArguments()[0];
                if (listType.IsInterface)
                    return true;
            }

            return false;
        }

        public override int ProvideMemberHash(MemberInfo memberInfo)
            => memberInfo.Name.GetHashCode();

        public override string Generate(MemberInfo info, IMemberHandler.GenerationEvent evt)
        {
            string backingObject = $"_{info.Name}__backingObject";
            Type type = null;

            if (info is FieldInfo fi)
                type= fi.FieldType.GetGenericArguments()[0];

            if (info is PropertyInfo pi)
                type = pi.PropertyType.GetGenericArguments()[0];
            
            switch (evt)
            {
                case IMemberHandler.GenerationEvent.RuntimeMembers:
                    return $"[SerializeField, JankInspectIgnore] List<UnityEngine.Object> {backingObject};";
                case IMemberHandler.GenerationEvent.RuntimeOnAfterDeserialize:
                    return $@"
                    // On Deserialization, set the interface
                    {info.Name} = {backingObject}.Select(o => o as {type.Namespace}.{type.Name}).ToList();";
            }

            return string.Empty;
        }

        public override void Render(VisualElement root, object instance, SerializedObject serializedObject, Type type, MemberInfo member)
        {
            UTCustomEditor.InspectInterfaceListBackedByMonoBehaviour(root, serializedObject, instance, $"_{member.Name}__backingObject", $"{member.Name}", type);
        }
    }
}
#endif
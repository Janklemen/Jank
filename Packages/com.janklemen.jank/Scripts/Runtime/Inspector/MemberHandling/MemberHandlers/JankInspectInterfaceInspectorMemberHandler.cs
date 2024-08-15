#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine.UIElements;

namespace Jank.Inspector.CustomEditorGenerator
{
    public class JankInspectInterfaceInspectorMemberHandler : AMemberHandler
    {
        public override bool CanHandleMember(MemberInfo member)
        {
            if (!member.GetCustomAttributes().Any(a => a is JankInspectAttribute))
                return false;

            if (member is FieldInfo fi && fi.FieldType.IsInterface)
                return true;

            if (member is PropertyInfo pi && pi.PropertyType.IsInterface)
                return true;

            return false;
        }

        public override int ProvideMemberHash(MemberInfo memberInfo)
            => memberInfo.Name.GetHashCode();

        public override string Generate(MemberInfo info, IMemberHandler.GenerationEvent evt)
        {
            string backingObject = $"_{info.Name}__backingObject";

            Type type = null;

            if (info is FieldInfo fi)
                type = fi.FieldType;

            if (info is PropertyInfo pi)
                type = pi.PropertyType;

            switch (evt)
            {
                case IMemberHandler.GenerationEvent.RuntimeMembers:
                    return $"[SerializeField, JankInspectIgnore] UnityEngine.Object {backingObject};";
                case IMemberHandler.GenerationEvent.RuntimeOnAfterDeserialize:
                    return $@"
                    // On Deserialization, set the interface
                    {info.Name} = {backingObject} as {type.Namespace}.{type.Name};";
            }

            return string.Empty;
        }

        public override void Render(VisualElement root, object instance, SerializedObject serializedObject, Type type, MemberInfo member)
        {
            UTCustomEditor.InspectInterfaceBackedByMonoBehaviour(root, serializedObject, instance, $"_{member.Name}__backingObject", $"{member.Name}", type);
        }
    }
}
#endif
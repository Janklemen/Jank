#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine.UIElements;

namespace Jank.Inspector.CustomEditorGenerator
{
    /// <summary>
    /// Uses reflection to inspect an object and creates a custom inspector for it. Tries to show any object in a
    /// reasonable way
    /// </summary>
    public class JankInspectObjectInspectorMemberHandler : AMemberHandler
    {
        public override bool CanHandleMember(MemberInfo member)
        {
            if (!member.GetCustomAttributes().Any(a => a is JankInspectAttribute))
                return false;

            return member is FieldInfo or PropertyInfo;
        }

        public override int ProvideMemberHash(MemberInfo memberInfo)
            => memberInfo.Name.GetHashCode();

        public override string Generate(MemberInfo info, IMemberHandler.GenerationEvent evt)
            => string.Empty;

        public override void Render(VisualElement root, object instance, SerializedObject serializedObject, Type type, MemberInfo member)
        {
            string mode = member is FieldInfo ? "Field" : "Property";
            
            UTCustomEditor.InspectUsingObjectInspector(root, instance, $"{member.Name}", $"{mode}");
        }
    }
}
#endif
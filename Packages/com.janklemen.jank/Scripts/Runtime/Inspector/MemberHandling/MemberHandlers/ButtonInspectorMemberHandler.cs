#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine.UIElements;

namespace Jank.Inspector.CustomEditorGenerator
{
    public class ButtonInspectorMemberHandler : AMemberHandler
    {
        public override bool CanHandleMember(MemberInfo member)
        {
            if (!member.GetCustomAttributes().Any(a => a is JankInspectAttribute))
                return false;

            if (member is not MethodInfo info)
                return false;

            if (info.ReturnType != typeof(void) && info.ReturnType != typeof(UniTask))
                return false;

            return true;
        }

        public override int ProvideMemberHash(MemberInfo memberInfo)
            => memberInfo.Name.GetHashCode();

        public override string Generate(MemberInfo info, IMemberHandler.GenerationEvent evt) => string.Empty;

        public override void Render(VisualElement root, object instance, SerializedObject serializedObject, Type type, MemberInfo member)
        {
            UTCustomEditor.InspectMethod(root, serializedObject, instance,  instance.GetType().GetMethod($"{member.Name}", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic));
        }
    }
}
#endif
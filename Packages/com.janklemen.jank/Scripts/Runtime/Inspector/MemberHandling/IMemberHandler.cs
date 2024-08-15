#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace Jank.Inspector.CustomEditorGenerator
{
    public interface IMemberHandler
    {
        public enum GenerationEvent
        {
            RuntimeMembers,
            RuntimeOnAfterDeserialize
        }
        
        public bool CanHandleMember(MemberInfo member);
        public int ProvideMemberHash(MemberInfo memberInfo);
        public string Generate(MemberInfo info, GenerationEvent evt);
        public void Render(VisualElement root, object instance, SerializedObject serializedObject, Type type,
            MemberInfo member);
    }
}
#endif
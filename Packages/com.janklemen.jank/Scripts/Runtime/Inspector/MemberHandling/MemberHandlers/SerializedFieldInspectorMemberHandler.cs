#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector.CustomEditorGenerator
{
    /// <summary>
    /// Creates a serialized property from members that are normally serialized by Unity. 
    /// </summary>
    public class SerializedFieldInspectorMemberHandler : AMemberHandler
    {
        public override bool CanHandleMember(MemberInfo member)
        {
            if (!(member is FieldInfo field))
                return false;

            bool correctKeywords = (field.IsPublic && !field.IsStatic && !field.IsInitOnly)
                                   || field.GetCustomAttributes().Any(a => a is SerializeField);

            bool isUnitySerializableType =
                field.FieldType.IsPrimitive ||
                field.FieldType == typeof(string) ||
                field.FieldType == typeof(Vector3) ||
                field.FieldType == typeof(Vector2) ||
                field.FieldType == typeof(Vector4) ||
                field.FieldType == typeof(Quaternion) ||
                field.FieldType == typeof(Color) ||
                field.FieldType == typeof(Rect) ||
                field.FieldType.IsEnum ||
                typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType);

            return correctKeywords && isUnitySerializableType;
        }

        public override int ProvideMemberHash(MemberInfo memberInfo)
            => memberInfo.Name.GetHashCode();

        public override string Generate(MemberInfo info, IMemberHandler.GenerationEvent evt)
            => string.Empty;

        public override void Render(VisualElement root, object instance, SerializedObject serializedObject, Type type, MemberInfo member)
        {
            UTCustomEditor.InspectProperty(root, serializedObject, $"{member.Name}");
        }
    }
}
#endif
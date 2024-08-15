#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jank.Inspector.CustomEditorGenerator;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Jank.Inspector
{
    public static class UTCustomEditor
    {
        public static void InspectObject(VisualElement parent, object instance, SerializedObject serializedObject, Type type)
        {
            parent.Add(new JankCustomEditor(instance, serializedObject, type));
        }

        public static void InspectUsingObjectInspector(VisualElement parent, object instance, string memberName,
            string mode)
        {
            object obj = null;

            if (mode == "Field")
                obj = instance.GetType().GetField(
                        memberName,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(instance);

            if (mode == "Property")
                obj = instance.GetType().GetProperty(
                        memberName,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(instance);

            // Show inspector for interface {member.Name}
            if (obj == null)
            {
                DrawNullObject(parent, memberName);
                return;
            }

            parent.Add(new InspectableValue(memberName, obj.GetType(), obj));
        }

        public static void InspectInterfaceBackedByMonoBehaviour(VisualElement root,
            SerializedObject serializedObject, object instance,
            string backingObjectName, string memberName, Type targetType)
        {
            var serializedProperty = serializedObject.FindProperty(backingObjectName);

            if (serializedProperty != null)
            {
                PropertyField propertyField = new(serializedProperty, $"{memberName}:{targetType.Name}");
                propertyField.BindProperty(serializedProperty);
                root.Add(propertyField);
            }
            else
            {
                root.Add(new Label($"Could not find object {backingObjectName}"){style = { color = Color.red}});
                return;
            }
            
            Object backingObject = serializedProperty.objectReferenceValue;

            object interfaceInstance = null;

            if (targetType.IsInstanceOfType(backingObject))
            {
                if (backingObject is ScriptableObject so)
                    interfaceInstance = so;

                if (backingObject is MonoBehaviour mo)
                    interfaceInstance = mo?.GetComponent(targetType);

                if (backingObject is GameObject go)
                    interfaceInstance = go?.GetComponent(targetType);
            }

            serializedProperty.objectReferenceValue = interfaceInstance as Object;

            if (interfaceInstance != null)
            {
                FieldInfo field = instance.GetType().GetField(memberName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (field != null)
                    field.SetValue(instance, interfaceInstance);

                PropertyInfo property = instance.GetType().GetProperty(memberName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (property != null)
                    property.SetValue(instance, interfaceInstance);
            }
        }

        public static void InspectInterfaceListBackedByMonoBehaviour(VisualElement parent,
            SerializedObject serializedObject,
            object instance, string backingObjectName, string memberName, Type targetType)
        {
            SerializedProperty serializedProperty = serializedObject.FindProperty(backingObjectName);

            if (serializedProperty != null && serializedProperty.isArray)
            {
                PropertyField propertyField = new(serializedProperty, memberName + $" (List<{targetType.Name}>)");
                propertyField.BindProperty(serializedProperty);
                parent.Add(propertyField);

                for (int i = 0; i < serializedProperty.arraySize; i++)
                {
                    Object backingObject = serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                    object interfaceInstance = null;

                    if (backingObject is ScriptableObject so && targetType.IsAssignableFrom(so.GetType()))
                        interfaceInstance = so;

                    if (backingObject is MonoBehaviour mo && targetType.IsAssignableFrom(mo.GetType()))
                        interfaceInstance = mo.GetComponent(targetType);

                    if (backingObject is GameObject go && targetType.IsAssignableFrom(go.GetType()))
                        interfaceInstance = go.GetComponent(targetType);

                    serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue = interfaceInstance as Object;

                    if (interfaceInstance != null)
                    {
                        FieldInfo field = instance.GetType().GetField(memberName,
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                        if (field != null)
                        {
                            IList list = field.GetValue(instance) as IList;

                            if (list != null && list.Count > i)
                                list[i] = interfaceInstance;
                        }

                        PropertyInfo property = instance.GetType().GetProperty(memberName,
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                        if (property != null)
                        {
                            IList list = property.GetValue(instance) as IList;

                            if (list != null && list.Count > i)
                                list[i] = interfaceInstance;
                        }
                    }
                }
            }
        }

        public static void InspectProperty(VisualElement root, SerializedObject serializedObject, string propertyName)
        {
            var serializedProperty = serializedObject.FindProperty(propertyName);

            if (serializedProperty != null)
            {
                PropertyField propertyField = new(serializedProperty, propertyName);
                root.Add(propertyField);
            }
        }

        public static void InspectMethod(VisualElement root, SerializedObject serializedObject, object instance,
            MethodInfo method)
        {
            var element = new VisualElement();

            // Add border
            element.style.borderTopWidth = 1f;
            element.style.borderTopColor = new StyleColor(Color.red);
            element.style.borderRightWidth = 1f;
            element.style.borderRightColor = new StyleColor(Color.red);
            element.style.borderBottomWidth = 1f;
            element.style.borderBottomColor = new StyleColor(Color.red);
            element.style.borderLeftWidth = 1f;
            element.style.borderLeftColor = new StyleColor(Color.red);
            element.style.backgroundColor = new StyleColor(Color.grey);

            root.Add(element);
            root = element;

            var parameters = method.GetParameters();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    TypeConstructor typeConstructor = new TypeConstructor(parameter.Name, parameter.ParameterType);
                    root.Add(typeConstructor);

                    Button b = new() { text = "Constrcturo" };
                    b.clicked += () => { Debug.LogWarning(typeConstructor.Construct()); };
                    root.Add(b);
                }
            }

            Button button = new(() => method.Invoke(instance, null));
            button.text = method.Name;
            root.Add(button);
        }

        public static void DrawNullObject(VisualElement parent, string name)
        {
            Label label = new Label($"{name} = default");
            label.SetEnabled(false);
            parent.Add(label);
        }
    }
}
#endif
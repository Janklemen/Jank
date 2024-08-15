#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Jank.Inspector.CustomEditorGenerator;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jank.Inspector
{
    public class TypeConstructor : VisualElement
    {
        TabView _menu = new();
        ConstructorInfo[] _constructors;

        public TypeConstructor(string memberName, Type type)
        {
            SetData(memberName, type);
        }

        public void SetData(string memberName, Type type)
        {
            Clear();

            if (type == null)
                return;

            Add(new Label($"Type Constructor: {memberName}"));

            _constructors =
                type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Create a new VisualElement
            _menu.Clear();

            int i = 1;
            foreach (ConstructorInfo constructor in _constructors)
            {
                // Create a new VisualElement for each constructor
                var subMenu = new Tab($"Con{i++}")
                {
                    name = constructor.Name,
                };

                foreach (var param in constructor.GetParameters())
                {
                    subMenu.Add(new InspectableValue(param.Name, param.ParameterType, enabled: true));
                }

                // Add the subMenu to the main menu
                _menu.Add(subMenu);
            }

            Add(_menu);
        }

        public object Construct()
        {
            // Assume that a default value exists
            Tab activeTab = _menu.activeTab;
            if (activeTab == null)
            {
                return null;
            }

            // Find the corresponding constructor
            ConstructorInfo constructorToUse = null;
            foreach (ConstructorInfo constructor in _constructors)
            {
                if (constructor.Name == activeTab.name)
                {
                    constructorToUse = constructor;
                    break;
                }
            }

            if (constructorToUse == null)
            {
                return null;
            }

            // Get the parameter info
            ParameterInfo[] parameters = constructorToUse.GetParameters();
            object[] parameterValues = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                // Assume that each element in the tab corresponds to a parameter in the constructor
                InspectableValue value = (InspectableValue)activeTab.ElementAt(i);
                parameterValues[i] = Convert.ChangeType(value.Value, parameters[i].ParameterType);
            }

            return constructorToUse.Invoke(parameterValues);
        }
    }
}
#endif
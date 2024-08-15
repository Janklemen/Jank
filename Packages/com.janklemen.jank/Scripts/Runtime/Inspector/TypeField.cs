#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace Jank.Inspector
{
    public class TypeField : VisualElement
    {
        public void SetData(string memberName, Type type)
        {
            Clear();

            if (type == null)
                return;

            ConstructorInfo[] constructors =
                type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Create a new TabView
            var tabView = new TabView();

            foreach (ConstructorInfo constructor in constructors)
            {
                // Create a new tab for each constructor
                var item = new Tab
                {
                    name = constructor.Name,
                };

                // create a label with constructor's info for tab content
                string constructorInfo = $"Constructor: {constructor}";
                foreach (var param in constructor.GetParameters())
                {
                    constructorInfo += $"\nArgument: {param.Name} of type {param.ParameterType}";
                }

                var info = new Label(constructorInfo);
                item.Add(info);

                // Add the tab to TabView
                tabView.Add(item);
            }

            Add(tabView);
        }

        void DrawValue(string name, object value)
        {
            if (name.Contains("k__BackingField"))
                return;

            if (value == null)
            {
                UTCustomEditor.DrawNullObject(this, name);
                return;
            }

            Type type = value.GetType();

            Add(new InspectableValue(name, value.GetType(), value));
        }
    }
}
#endif
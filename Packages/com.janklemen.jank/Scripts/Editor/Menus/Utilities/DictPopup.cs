using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jank.Editor.Menus.Utilities
{
    public class DictPopup : PopupWindowContent
    {
        Dictionary<string, string> _input;
        readonly System.Action<Dictionary<string, string>> _onSubmit;

        public DictPopup(Dictionary<string, string> inputs, System.Action<Dictionary<string, string>> onSubmit)
        {
            _input = inputs;
            _onSubmit = onSubmit;
        }

        public override void OnGUI(Rect rect)
        {
            Dictionary<string, string> collect = new();
            
            foreach (KeyValuePair<string, string> pair in _input)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(pair.Key);
                collect[pair.Key] = GUILayout.TextField(_input[pair.Key], GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();
            }

            _input = collect;

            if (GUILayout.Button("Submit"))
            {
                _onSubmit(collect);
                editorWindow.Close();
            }
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(600, 300);
        }

        public static Rect DefaultRect() => new Rect(0, 0, 600, 300);
    }
}
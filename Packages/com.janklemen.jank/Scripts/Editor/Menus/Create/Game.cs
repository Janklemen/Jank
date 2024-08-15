using System.Collections.Generic;
using Jank.Editor.Menus.Utilities;
using UnityEditor;

namespace Jank.Editor.Menus.Create
{
    public static class Game
    {
        [MenuItem("Assets/Create/Jank/Game/AssemblyDefinition", false, 0)]
        private static void CreateAsmdef()
        {
            Dictionary<string, string> inputs = new() { { "GameName", "Game" } };
            DictPopup popup = new(inputs, Generate);
            PopupWindow.Show(DictPopup.DefaultRect(), popup);
            
            void Generate(Dictionary<string, string> args) => AsmdefSaveFile(args["GameName"]);
        }

        static void AsmdefSaveFile(string name)
        {
            ProjectWindowUtil.CreateAssetWithContent($"Janklemen.{name}.asmdef", Templates.AsmDefTemplate(name));
        } 
        
        [MenuItem("Assets/Create/Jank/Game/Runner", false, 1)]
        static void RunnerMenuItem()
        {
            Dictionary<string, string> inputs = new() { { "GameName", "Game" } };
            DictPopup popup = new(inputs, Generate);
            PopupWindow.Show(DictPopup.DefaultRect(), popup);
            
            void Generate(Dictionary<string, string> args) => RunnerSaveFile(args["GameName"]);
        }

        static void RunnerSaveFile(string name)
        {
            ProjectWindowUtil.CreateAssetWithContent($"{name}Runner.cs", Templates.RunnerTemplate(name));
        } 
        
        [MenuItem("Assets/Create/Jank/Game/Rulebook", false, 2)]
        static void RulebookMenuItem()
        {
            Dictionary<string, string> inputs = new() { { "GameName", "Game" } };
            DictPopup popup = new(inputs, Generate);
            PopupWindow.Show(DictPopup.DefaultRect(), popup);
            
            void Generate(Dictionary<string, string> args) => RulebookSaveFile(args["GameName"]);
        }

        static void RulebookSaveFile(string name)
        {
            ProjectWindowUtil.CreateAssetWithContent($"{name}Rulebook.cs", Templates.RulebookTemplate(name));
            ProjectWindowUtil.CreateAssetWithContent($"{name}State.cs", Templates.StateTemplate(name));
        } 
        
        [MenuItem("Assets/Create/Jank/Game/Project", false, -1)]
        static void ManuItemProject()
        {
            Dictionary<string, string> inputs = new() { { "GameName", "Game" } };
            DictPopup popup = new(inputs, Generate);
            PopupWindow.Show(DictPopup.DefaultRect(), popup);

            void Generate(Dictionary<string, string> args)
            {
                string name = args["GameName"];
                AsmdefSaveFile(name);
                RunnerSaveFile(name);
                RulebookSaveFile(name);
                
            }
        }
    }
}
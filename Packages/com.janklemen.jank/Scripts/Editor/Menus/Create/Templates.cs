namespace Jank.Editor.Menus.Create
{
    public static class Templates
    {
        public static string AsmDefTemplate(string name)
            => $@"
{{
    ""name"": ""{name}"",
    ""rootNamespace"": ""{name}"",
    ""references"": [
        ""Jank"",
        ""UniTask""
    ],
    ""includePlatforms"": [],
    ""excludePlatforms"": [],
    ""allowUnsafeCode"": false,
    ""overrideReferences"": false,
    ""precompiledReferences"": [],
    ""autoReferenced"": true,
    ""defineConstraints"": [],
    ""versionDefines"": [],
    ""noEngineReferences"": false
}}
";
        
        public static string RunnerTemplate(string name)
            => $@"
using Jank.App;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Janklemen.{name}
{{
    public class {name}Runner : AJankRunner
    {{
        [SerializeField]
        {name}State State;

        [SerializeField]
        {name}Rulebook Rulebook;

        public override async UniTask Run()
        {{
            if(State == null)
                State = ScriptableObject.CreateInstance<{name}State>();
            else
                State = Instantiate(State);

            await Rulebook.Run(State);
        }}
    }}
}}
";
        
        public static string StateTemplate(string name)
            => $@"
using Jank.Inspector.CustomEditorGenerator;
using Jank.States;
using UnityEngine;

namespace Janklemen.{name}
{{
    [JankState]
    [JankCustomEditor]
    [CreateAssetMenu(menuName = ""Dev/{name}/State"", fileName = ""{name}State"")]
    public partial class {name}State : ScriptableObject
    {{
        [JankInspect] E{name}Rule ActiveRule;
        [JankInspect] E{name}Rule LastRule;
    }}
}}
";

        public static string RulebookTemplate(string name)
            => $@"
using Cysharp.Threading.Tasks;
using Jank.States;
using UnityEngine;

namespace Janklemen.{name}
{{  
    [JankRules]
    public enum E{name}Rule
    {{
        /// <summary>
        /// Rule
        /// </summary>
        Rule,
        Terminate
    }}
    
    public partial class {name}Rulebook: A{name}Rulebook
    {{
        protected override UniTask<E{name}Rule> RunRule({name}State state)
        {{
            return UniTask.FromResult(EHexGridRule.Terminate);
        }}
    }}
}}
";


    }
}
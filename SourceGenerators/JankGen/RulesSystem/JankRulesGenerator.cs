using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JankGen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen
{
    [Generator]
    public class JankRulesGenerator : AJankGenerator
    {
        const string cAttributeName = "JankRules";

        public override void ExecuteClassGenerator(GeneratorExecutionContext context,
            Compilation compilation, SemanticModel model,
            ClassDeclarationSyntax classDeclarationSyntax)
        {
        }

        public override void ExecuteEnumGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model, EnumDeclarationSyntax enumDeclarationSyntax)
        {
            if (!enumDeclarationSyntax.IsWithAttribute(cAttributeName))
                return;

            NamespaceDeclarationSyntax namespa = enumDeclarationSyntax.Parent as NamespaceDeclarationSyntax;

            string namespaceName = namespa!.Name.ToString();
            string className = enumDeclarationSyntax.Identifier.ToString();

            IEnumerable<string> machineStates = enumDeclarationSyntax.Members
                .Select(e => e.Identifier.Text)
                .Where(e => e != "Terminate");

            List<string> usings = new();
            usings.Add("System");
            usings.Add("System.Threading");
            usings.Add("System.Collections.Generic");
            usings.Add("UnityEngine");
            usings.Add("Jank.Utilities");
            usings.Add("Jank.App");
            usings.Add("Cysharp.Threading.Tasks");
            usings.Add("Jank.Utilities.Disposing");
            usings.Add("Jank.Objects");

            string abstractName = className.Substring(1);
            string stateName = Regex.Replace(abstractName, "Rule", "State");

            IEnumerable<string> stateMethods = machineStates
                .Select(m => $"protected abstract UniTask<{className}> Run{m}({stateName} state);");
            
            IEnumerable<string> stateCases = machineStates
                .Select(m =>
                {
                    return @$"
case {className}.{m}:
{{
    {className} nextRule = await Run{m}(State);
    await State.SetLastRule({className}.{m});
    await State.SetActiveRule(nextRule);
    break;
}}";
                });
            
            string classGeneratedSourceCode = $@"
{UTOutput.AggregateUsings(usings)}

namespace {namespaceName} 
{{
    public abstract class A{abstractName}book : MonoBehaviour
    {{
        /// <summary>
        /// The state that this state machine is currently using
        /// <summary>
        {stateName} State;

        /// <summary>
        /// Automatically disposes disposables after post run
        /// </summary>
        protected DisposableManager Disposables = new();

        /// <summary>
        /// During PreRun, state machines must read the state provided and immediately setup all objects
        /// required by the state. That means that all visuals should be set up immediately without
        /// animations. 
        /// </summary>
        protected virtual UniTask PreRun({stateName} state)=> UniTask.CompletedTask;

        /// <summary>
        /// Called in the terminate function before all Disposables are disposed
        /// </summary>
        protected virtual UniTask PostRun({stateName} state)=> UniTask.CompletedTask;

        /// <summary>
        /// Called if an exception is caught within the state machine
        /// </summary>
        protected virtual void HandleRunException({stateName} state, Exception e)=> throw e;

        /// <summary>
        /// Runs the statemachine, calling all lifecycle functions. States are called based on the rules implemented by
        /// the concrete class.
        /// </summary>
        public async UniTask Run({stateName} state = default)
        {{
            if(state != default)
                State = state;

            await PreRun(State);

            try
            {{
                while(true)
                {{
                    switch(State.GetActiveRule())
                    {{
                        {UTOutput.AggregateString(stateCases)}
                        case {className}.Terminate:
                        {{
                            await PostRun(State);
                            Disposables.Dispose();
                            return;
                        }}
                    }}                
                }}
            }}
            catch(Exception e)
            {{
                HandleRunException(State, e);
            }}
        }}

        {UTOutput.AggregateString(stateMethods)}
    }}
}}
";
            generatorExecutionContext.AddSource($"{abstractName}.rules.g",
                UTOutput.ArrangeUsingRoslyn(classGeneratedSourceCode));
        }

        public override void ExecutePostGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model)
        {
        }
    }
}
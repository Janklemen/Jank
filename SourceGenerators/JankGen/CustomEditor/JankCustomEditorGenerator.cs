using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Jank.Inspector.CustomEditorGenerator;
using JankGen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen
{
    [Generator]
    public class JankCustomEditorGenerator : AJankGenerator
    {
        const string cAttributeName = "JankCustomEditor";

        public override void ExecuteClassGenerator(GeneratorExecutionContext context,
            Compilation compilation, SemanticModel model,
            ClassDeclarationSyntax classDeclarationSyntax)
        {
            if (!classDeclarationSyntax.IsWithAttribute(cAttributeName))
                return;

            NamespaceDeclarationSyntax namespa = classDeclarationSyntax.Parent as NamespaceDeclarationSyntax;

            string namespaceName = namespa!.Name.ToString();
            string className = classDeclarationSyntax.Identifier.ToString();

            List<string> usings = new();
            usings.Add("Jank.Objects");
            usings.Add("System");
            usings.Add("System.Collections.Generic");
            usings.Add("System.Linq");
            usings.Add("System.Reflection");
            usings.Add("UnityEngine");
            usings.Add("UnityEngine.UIElements");
            usings.Add("UnityEditor");
            usings.Add("UnityEditor.UIElements");
            usings.Add("Jank.Inspector");
            usings.Add("Jank.Inspector.CustomEditorGenerator");

            string editorSourceCode = $@"
#if UNITY_EDITOR
{UTOutput.AggregateUsings(usings)}

namespace {namespaceName} 
{{
    [CustomEditor(typeof({className}))]
    public class {className}Editor : UnityEditor.Editor
    {{
        public override VisualElement CreateInspectorGUI()
        {{
            VisualElement root = new();
            {className} instance = ({className})target;
            SerializedObject obj = this.serializedObject;

            root.Add(new JankCustomEditor(instance, obj, typeof({className})));

            root.Bind(serializedObject);
            return root;
        }}
    }}
}}
#endif
";
            context.AddSource($"{className}.jankeditor.editor.g",
                UTOutput.ArrangeUsingRoslyn(editorSourceCode));

            // Deal with the runtime piece
            INamedTypeSymbol classSymbol = model.GetDeclaredSymbol(classDeclarationSyntax) as INamedTypeSymbol;
            Type type = Type.GetType(classSymbol?.ToDisplayString());
            
            StringBuilder members = new();
            StringBuilder deserializers = new();
            
            foreach (ISymbol member in classDeclarationSyntax.GetAllMembers(context, true))
            {
                if (UTMemberHandler.TryGetMemberHandler(member, out IMemberHandler handler))
                {
                    members.AppendLine(handler.Generate(member, IMemberHandler.GenerationEvent.RuntimeMembers));
                    deserializers.AppendLine(handler.Generate(member, IMemberHandler.GenerationEvent.RuntimeOnAfterDeserialize));
                }
            }
            
            string classGeneratedSourceCode = $@"
using Jank.Inspector.CustomEditorGenerator;
using Jank.Objects;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace {namespaceName} 
{{
    public partial class {className} : ISerializationCallbackReceiver
    {{
        {members}

        public void OnBeforeSerialize() {{
        
        }}

        public void OnAfterDeserialize() {{
            {deserializers}
        }}
    }}
}}
";


            context.AddSource($"{className}.jankeditor.runtime.g",
                UTOutput.ArrangeUsingRoslyn(classGeneratedSourceCode));
        }

        public override void ExecuteEnumGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model, EnumDeclarationSyntax enumDeclarationSyntax)
        {
        }

        public override void ExecutePostGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model)
        {
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using JankGen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace JankGen
{
    [Generator]
    public class JankVisualElementGenerator : AJankGenerator
    {
        const string cAttributeName = "JankVisualElement";

        public override void ExecuteClassGenerator(GeneratorExecutionContext context,
            Compilation compilation,
            SemanticModel model, ClassDeclarationSyntax classDeclarationSyntax)
        {
            if (!classDeclarationSyntax.IsWithAttribute(cAttributeName))
                return;

            NamespaceDeclarationSyntax namespa = classDeclarationSyntax.Parent as NamespaceDeclarationSyntax;
            string namespaceName = namespa.Name.ToString();
            string className = classDeclarationSyntax.Identifier.ToString();


            List<PropertyDeclarationSyntax> validFields = classDeclarationSyntax.GetValidProperties(
                p => p.AttributeLists
                    .SelectMany(a => a.Attributes)
                    .Any(a => a.GetText().ToString().Contains("JankVisualAttribute")));

            GenerateAttributeObjects(className, validFields, out List<string> attributeDeclarations,
                out List<string> attributeInitCalls);

            string template = $@"
using Jank.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace {namespaceName} 
{{
    public partial class {className}
    {{
        public {className}()
        {{
            var visualTree = Resources.Load<VisualTreeAsset>(""Layouts/{className}"");
            visualTree.CloneTree(this);

            Init();
        }}
        
        public new class UxmlFactory : UxmlFactory<{className}, UxmlTraits> {{ }}

        public new class UxmlTraits : VisualElement.UxmlTraits
        {{
            {UTOutput.AggregateString(attributeDeclarations)}

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {{
                base.Init(ve, bag, cc);
                {UTOutput.AggregateString(attributeInitCalls)}
            }}
        }}        
    }}
}}
";

            context.AddSource($"{classDeclarationSyntax.Identifier}.visualelement.g",
                UTOutput.ArrangeUsingRoslyn(template));
        }

        public override void ExecuteEnumGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model, EnumDeclarationSyntax enumDeclarationSyntax)
        {
        }

        void GenerateAttributeObjects(string className, List<PropertyDeclarationSyntax> validFields,
            out List<string> attributeDeclarations, out List<string> attributeInitCalls)
        {
            attributeDeclarations = new();
            attributeInitCalls = new();

            foreach (PropertyDeclarationSyntax fieldDeclarationSyntax in validFields)
            {
                string name = fieldDeclarationSyntax.Identifier.ToString();
                var type = fieldDeclarationSyntax.Type.GetText();

                string typeStr = TypeToTypeString(type);
                string attributeName = ProcessAttributeName(name.Trim());
                string attributeIdentifier = $"m_{attributeName}";

                string attributeDeclaration =
                    $@"readonly Uxml{typeStr}AttributeDescription {attributeIdentifier} = new() {{ name = ""{attributeName}"" }};";
                attributeDeclarations.Add(attributeDeclaration);

                string attributeInit =
                    $@"(({className})ve).{attributeName} = {attributeIdentifier}.GetValueFromBag(bag, cc);";
                attributeInitCalls.Add(attributeInit);
            }
        }

        string ProcessAttributeName(string name)
        {
            if (name.StartsWith("_"))
                return char.ToUpper(name[1]) + name.Substring(2);

            return char.ToUpper(name[0]) + name.Substring(1);
        }

        string TypeToTypeString(SourceText type)
        {
            switch (type.ToString().Trim())
            {
                case "int": return "Int";
                case "float": return "Float";
                case "string": return "String";
            }

            return type.ToString();
        }

        public override void ExecutePostGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model)
        {
        }
    }
}
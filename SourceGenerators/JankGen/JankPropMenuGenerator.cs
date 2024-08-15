using System;
using System.Collections.Generic;
using System.Linq;
using JankGen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen
{
    [Generator]
    public class JankPropMenuGenerator : AJankGenerator
    {
        private const string cAttributeName = "JankPropMenuItemAttribute";

        public override void ExecuteClassGenerator(GeneratorExecutionContext context, Compilation compilation, SemanticModel model, ClassDeclarationSyntax classDeclarationSyntax)
        {
            INamedTypeSymbol classSymbol = model.GetDeclaredSymbol(classDeclarationSyntax);
            AttributeData attributeData = classSymbol.GetAttributes().FirstOrDefault(ad => ad.AttributeClass.Name == cAttributeName);

            if (attributeData == null)
                return;

            string namespaceName = classDeclarationSyntax.GetNearestNamespaceName();
            string className = classDeclarationSyntax.Identifier.ToString();
    
            // Retrieve additional class names from the attribute's params string[] argument
            var additionalClassNames = attributeData.ConstructorArguments
                .Where(arg => arg.Values != null && arg.Values.Length > 0)
                .SelectMany(arg => arg.Values.Select(v => v.Value?.ToString()))
                .Where(name => name != null)
                .ToList();

            // Generate menu item for the main class
            GenerateMenuItemClass(context, namespaceName, className, className);
            
            // Generate menu items for additional classes specified in the attribute
            foreach (string additionalClassName in additionalClassNames)
            {
                GenerateMenuItemClass(context, namespaceName, additionalClassName, additionalClassName);
            }
        }

        private void GenerateMenuItemClass(GeneratorExecutionContext context, string namespaceName, string className, string menuItemName)
        {
            string classGeneratedSourceCode = $@"
#if UNITY_EDITOR
using UnityEditor;
using Jank.Props.Utilities;

namespace {namespaceName}
{{
    public static class {className}MenuItems
    {{
        [MenuItem(""GameObject/Jank/{menuItemName}"", false, 0)]
        public static void Create()
        {{
            UTPropMenus.CreateObject(""{className}"");
        }}
    }}
}}
#endif
";
            context.AddSource($"{className}.jankpropmenuitem.g", UTOutput.ArrangeUsingRoslyn(classGeneratedSourceCode));
        }

        public override void ExecuteEnumGenerator(GeneratorExecutionContext context, Compilation compilation, SemanticModel model, EnumDeclarationSyntax enumDeclarationSyntax)
        {
            // No enum generation needed for this generator
        }

        public override void ExecutePostGenerator(GeneratorExecutionContext context, Compilation compilation, SemanticModel model)
        {
            // No post-generation actions needed for this generator
        }

        class SyntaxReceiver : ISyntaxReceiver
        {
            public List<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax classDeclaration &&
                    classDeclaration.AttributeLists.Count > 0)
                {
                    CandidateClasses.Add(classDeclaration);
                }
            }
        }
    }
}

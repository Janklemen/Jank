using System;
using System.Collections.Generic;
using System.Linq;
using JankGen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen
{
    /// <summary>
    /// Any class that has field using the JankInject attribute is a jank injectable and participates in the
    /// injection lifecycle. 
    /// </summary>
    [Generator]
    public class JankInjectableGenerator : AJankGenerator
    {
        public override void ExecuteClassGenerator(GeneratorExecutionContext context,
            Compilation compilation, SemanticModel model,
            ClassDeclarationSyntax classDeclarationSyntax)
        {
            // .g is used to indicate a file is generated. Don't operate on those
            if(classDeclarationSyntax.SyntaxTree.FilePath.Contains(".g"))
                return;
            
            if (classDeclarationSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.AbstractKeyword)))
                return;

            string namespaceName = classDeclarationSyntax.GetNearestNamespaceName();
            string className = classDeclarationSyntax.Identifier.ToString();

            List<IFieldSymbol> singles = classDeclarationSyntax.GetAllFieldSymbols(context, true)
                .GetFieldsWithAttribute("JankInject");

            List<IFieldSymbol> labeled = classDeclarationSyntax.GetAllFieldSymbols(context, true)
                .GetFieldsWithAttribute("JankInjectLabeled");

            List<IFieldSymbol> fieldsToProcess =
                classDeclarationSyntax.GetAllFieldSymbols(context)
                    .Where(symbol => !singles.Contains(symbol) && !labeled.Contains(symbol))
                    .Where(f => !f.Name.Contains("k__BackingField"))
                    .ToList();
            
            // Create logic to handle fields in base classes
            INamedTypeSymbol classSymbol = model.GetDeclaredSymbol(classDeclarationSyntax);
            INamedTypeSymbol baseTypeSymbol = classSymbol?.BaseType;
            
            if (singles == null || singles.Count == 0)
                return;

            List<string> namesp = new(
                singles.SelectMany(f => f.CreateUsings())
            );

            namesp.AddRange(new[] { "System", "System.Collections.Generic", "Jank.Objects", "Jank.Objects" });

            List<string> singleStrings =
                singles.Select(CreateInjectableField).ToList();

            List<string> labeledStrings =
                labeled.Select(CreateLabeledField).ToList();

            List<string> injectableStrings =
                fieldsToProcess.Select(f => $"obm.ProcessObject({f.Name});").ToList();

            string classGeneratedSourceCode = $@"
{UTOutput.AggregateUsings(namesp)}

namespace {namespaceName} 
{{
    public partial class {className} : IJankInjectable
    {{
        public bool IsInjected {{ get; private set; }} = false;

        public void Inject(IObjectManager obm)
        {{
            {UTOutput.AggregateString(singleStrings)}
            {UTOutput.AggregateString(labeledStrings)}
            {UTOutput.AggregateString(injectableStrings)}
            if(this is IPostJankInject pji)
                pji.PostInject();
            IsInjected = true;
        }}
    }}
}}
";
            context.AddSource($"{classDeclarationSyntax.Identifier}.injectable.g",
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

        public string CreateInjectableField(IFieldSymbol fieldSymbol)
        {
            string typeName = fieldSymbol.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

            return $"{fieldSymbol.Name} = ({typeName})obm.Singles[typeof({typeName})];";
        }

        public string CreateLabeledField(IFieldSymbol fieldSymbol)
        {
            string typeName = fieldSymbol.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

            string label = fieldSymbol.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass.Name == "JankInjectLabeledAttribute")
                .NamedArguments.FirstOrDefault(kv => kv.Key == "label")
                .Value.Value.ToString();
            
            return
                $"{fieldSymbol.Name} = ({typeName})obm.Labeled[{label}];";
        }
    }
}
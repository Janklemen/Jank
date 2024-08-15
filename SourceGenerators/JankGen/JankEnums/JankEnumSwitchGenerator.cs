using System.Linq;
using JankGen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen
{
    [Generator]
    public class JankEnumSwitchGenerator : AJankGenerator
    {
        const string cAttributeName = "JankEnumSwitch";

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

            // TODO: Make a function that can
            (string start, string end) = enumDeclarationSyntax.GenerateEnclosingDeclarations();

            string enumName = enumDeclarationSyntax.Identifier.ToString();
            string utilityName = "UT" + enumName;

            string parameters = string.Join(",",
                enumDeclarationSyntax
                    .EnumMembers()
                    .Select(m => $"ActionAsync {m.Decapitialize()} = null")
            );

            string cases = string.Join("\n",
                enumDeclarationSyntax
                    .EnumMembers()
                    .Select(m => $"case {enumName}.{m}:\nif({m.Decapitialize()} != null)\n{m.Decapitialize()}();\nbreak;")
            );

            string classGeneratedSourceCode = $@"
using System;
using Jank.Utilities;

{start}
    {utilityName.StaticPartialClass()} 
    {{
        public static ActionAsync<{enumName}> Switch({parameters})
        {{
            return async val => {{
                switch(val){{
                    {cases}
                }}
            }};
        }}
    }}
{end}
";
            generatorExecutionContext.AddSource($"{enumDeclarationSyntax.FullDeclarationIdentifier()}.switch.g",
                UTOutput.ArrangeUsingRoslyn(classGeneratedSourceCode));
        }

        public override void ExecutePostGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model)
        {
        }
    }
}
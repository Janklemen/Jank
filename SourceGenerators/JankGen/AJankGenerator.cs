using System;
using System.Text;
using JankGen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen
{
    /// <summary>
    /// Most generators written for Jank follow a similar pattern. This abstract class perform the common wrapper for
    /// generators and provides a mechanism for showing error which helps with debugging.
    /// </summary>
    public abstract class AJankGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            Compilation compilation = context.Compilation;

            foreach (SyntaxTree syntaxTree in compilation.SyntaxTrees)
            {
                SemanticModel model = compilation.GetSemanticModel(syntaxTree);

                foreach (ClassDeclarationSyntax classDeclarationSyntax in syntaxTree.GetClasses())
                {
                    try
                    {
                        ExecuteClassGenerator(context, compilation, model, classDeclarationSyntax);
                    }
                    catch (Exception e)
                    {
                        context.AddSource($"{classDeclarationSyntax.Identifier.Text}_class_ERROR.g", ErrorProgram(e));
                    }
                }

                foreach (EnumDeclarationSyntax enumDeclarationSyntax in syntaxTree.GetEnums())
                {
                    try
                    {
                        ExecuteEnumGenerator(context, compilation, model, enumDeclarationSyntax);
                    }
                    catch (Exception e)
                    {
                        context.AddSource($"{enumDeclarationSyntax.Identifier.Text}_class_ERROR.g", ErrorProgram(e));
                    }
                }


                ExecutePostGenerator(context, compilation, model);
            }
        }

        string ErrorProgram(Exception e)
        {
            StringBuilder sb = new();

            sb.AppendLine(e.GetType().Name);
            sb.AppendLine(e.Message);
            sb.AppendLine(e.StackTrace);

            return sb.ToString();
        }

        public abstract void ExecuteClassGenerator(
            GeneratorExecutionContext context,
            Compilation compilation,
            SemanticModel model, 
            ClassDeclarationSyntax classDeclarationSyntax
            );

        public abstract void ExecuteEnumGenerator(
            GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model,
            EnumDeclarationSyntax enumDeclarationSyntax
            );

        public abstract void ExecutePostGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation, SemanticModel model);
    }
}
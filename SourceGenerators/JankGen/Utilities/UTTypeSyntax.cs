using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen.Utilities;

public static class UTTypeSyntax
{
    public static TypeDeclarationSyntax GetTypeDeclarationSyntax(TypeSyntax type, GeneratorExecutionContext context)
    {
        ITypeSymbol typeSymbol = context.Compilation.GetSemanticModel(type.SyntaxTree).GetTypeInfo(type).Type;
        if (typeSymbol != null)
        {
            SyntaxReference typeDeclarationSyntaxReference = typeSymbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (typeDeclarationSyntaxReference != null)
            {
                TypeDeclarationSyntax typeDeclarationSyntax =
                    typeDeclarationSyntaxReference.GetSyntax() as TypeDeclarationSyntax;
                return typeDeclarationSyntax;
            }
        }

        return null;
    }
}
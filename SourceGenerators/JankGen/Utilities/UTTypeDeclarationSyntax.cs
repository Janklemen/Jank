using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen.Utilities;

public static class UTTypeDeclarationSyntax
{
    public static bool SupportsInterface(this TypeDeclarationSyntax type, GeneratorExecutionContext context,
        string interfc)
    {
        try
        {
            if (context.Compilation.GetSemanticModel(type.SyntaxTree).GetDeclaredSymbol(type) is INamedTypeSymbol
                typeSymbol)
            {
                return typeSymbol.AllInterfaces.Any(interfaceSymbol => interfaceSymbol.Name == interfc);
            }

            return false;
        }
        catch (Exception e)
        {
            throw new Exception($"{type.Identifier.ToString()}");
        }
    }
}
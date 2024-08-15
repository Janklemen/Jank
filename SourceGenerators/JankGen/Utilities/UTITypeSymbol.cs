using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace JankGen.Utilities;

public static class UTITypeSymbol
{
    public static void GetTypeSymbolUsings(this ITypeSymbol typeSymbol, List<string> usings)
    {
        if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
        {
            string use = namedTypeSymbol.ContainingNamespace.ToString();

            if (!usings.Contains(use))
                usings.Add(use);

            if (namedTypeSymbol.IsGenericType)
            {
                foreach (ITypeSymbol symbolTypeArgument in namedTypeSymbol.TypeArguments)
                {
                    GetTypeSymbolUsings(symbolTypeArgument, usings);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace JankGen.Utilities;

public static class UTISymbol
{
    public static List<ISymbol> GetValidSymbols(this IEnumerable<ISymbol> symbols,
        Predicate<ISymbol> isValid = null)
    {
        if (isValid == null)
            isValid = _ => true;

        List<ISymbol> validSymbols = new();
        foreach (ISymbol symbol in symbols)
        {
            if (isValid(symbol))
            {
                validSymbols.Add(symbol);
            }
        }

        return validSymbols;
    }

    public static List<ISymbol> GetSymbolsWithAttribute(this IEnumerable<ISymbol> symbols,
        params string[] attributes)
    {
        return symbols.GetValidSymbols(ValidateSymbolHasAttribute);

        bool ValidateSymbolHasAttribute(ISymbol symbol)
        {
            if (symbol.GetAttributes() == null)
                return false;

            foreach (string attribute in attributes)
            {
                if (symbol.GetAttributes().Any(at => at.AttributeClass.Name == attribute))
                    return true;

                if (symbol.GetAttributes().Any(at => at.AttributeClass.Name == attribute + "Attribute"))
                    return true;
            }

            return false;
        }
    }

    public static IEnumerable<string> CreateUsings(this ISymbol symbol)
    {
        List<string> s = new();

        if (symbol is not ITypeSymbol typeSymbol) 
        {
            return s;
        }

        Queue<ITypeSymbol> processing = new();
        processing.Enqueue(typeSymbol);

        while (processing.Count > 0)
        {
            ITypeSymbol next = processing.Dequeue();
            s.Add(next.ContainingNamespace.ToString());

            if (next is INamedTypeSymbol ins)
            {
                foreach (ITypeSymbol insTypeArgument in ins.TypeArguments)
                    processing.Enqueue(insTypeArgument);
            }
        }

        return s.Distinct();
    }
}
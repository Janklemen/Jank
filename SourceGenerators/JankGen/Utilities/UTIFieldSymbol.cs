using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace JankGen.Utilities;

public static class UTIFieldSymbol
{
    public static List<IFieldSymbol> GetValidFields(this IEnumerable<IFieldSymbol> fields,
        Predicate<IFieldSymbol> isValid = null)
    {
        if (isValid == null)
            isValid = fieldSymbol => true;

        List<IFieldSymbol> validFields = new();
        foreach (var fieldSymbol in fields)
        {
            if (isValid(fieldSymbol))
            {
                validFields.Add(fieldSymbol);
            }
        }

        return validFields;
    }

    public static List<IFieldSymbol> GetFieldsWithAttribute(this IEnumerable<IFieldSymbol> fields,
        params string[] attributes)
    {
        return fields.GetValidFields(ValidateFieldHasSymbol);

        bool ValidateFieldHasSymbol(IFieldSymbol symbol)
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

    public static IEnumerable<string> CreateUsings(this IFieldSymbol fieldSymbol)
    {
        List<string> s = new();

        Queue<ITypeSymbol> processing = new();
        processing.Enqueue(fieldSymbol.Type);

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

        return s;
    }
}
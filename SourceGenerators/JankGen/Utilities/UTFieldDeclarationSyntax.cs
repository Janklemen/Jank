using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen.Utilities;

public static class UTFieldDeclarationSyntax
{
    public static List<FieldDeclarationSyntax> GetValidFields(this IEnumerable<FieldDeclarationSyntax> fields,
        Predicate<FieldDeclarationSyntax> isValid = null)
    {
        if (isValid == null)
            isValid = pds => true;

        List<FieldDeclarationSyntax> validFields = new();
        foreach (FieldDeclarationSyntax propertyDeclarationSyntax in fields)
        {
            if (isValid(propertyDeclarationSyntax))
                validFields.Add(propertyDeclarationSyntax);
        }

        return validFields;
    }

    public static SyntaxToken GetFieldDeclarationIdentifier(this FieldDeclarationSyntax syntax)
    {
        return syntax.Declaration.Variables.First().Identifier;
    }

    public static string GetAttributeArgument(this FieldDeclarationSyntax syntax, string attributeName)
    {
        var injectLabeledAttribute = syntax.AttributeLists
            .SelectMany(attributeList => attributeList.Attributes)
            .FirstOrDefault(attribute => attribute.Name.ToString() == attributeName);

        if (injectLabeledAttribute != null)
            return injectLabeledAttribute.ArgumentList?.Arguments.FirstOrDefault().ToString();

        return string.Empty;
    }

    public static IEnumerable<string> CreateUsings(this FieldDeclarationSyntax syntax, SemanticModel model)
    {
        List<string> s = new();

        Queue<ITypeSymbol> processing = new();
        processing.Enqueue(ModelExtensions.GetTypeInfo(model, syntax.Declaration.Type).Type);

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
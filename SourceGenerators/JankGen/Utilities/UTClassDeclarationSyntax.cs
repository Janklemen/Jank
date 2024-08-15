using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen.Utilities;

public static class UTClassDeclarationSyntax
{
    /// <summary>
    /// Get a collection of class declarations that have a specific attribute.
    /// </summary>
    /// <param name="allClasses">The collection of all class declarations.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>A collection of class declarations that have the specified attribute.</returns>
    public static IEnumerable<ClassDeclarationSyntax> GetClassesWithAttribute(
        this IEnumerable<ClassDeclarationSyntax> allClasses, string attributeName)
    {
        IEnumerable<ClassDeclarationSyntax> x = allClasses
            .Where(syntax => IsWithAttribute(syntax, attributeName));
        return x;
    }

    public static bool IsWithAttribute(this ClassDeclarationSyntax clas, string attributeName)
    {
        return clas.AttributeLists.IsAttributeInList(attributeName);
    }

    public static List<PropertyDeclarationSyntax> GetValidProperties(this ClassDeclarationSyntax classDeclarationSyntax,
        Predicate<PropertyDeclarationSyntax> isValidProperty = null)
    {
        if (isValidProperty == null)
            isValidProperty = pds => true;

        IEnumerable<PropertyDeclarationSyntax> properties = classDeclarationSyntax
            .DescendantNodes()
            .OfType<PropertyDeclarationSyntax>();

        List<PropertyDeclarationSyntax> validProperties = new();
        foreach (PropertyDeclarationSyntax propertyDeclarationSyntax in properties)
        {
            if (isValidProperty(propertyDeclarationSyntax))
                validProperties.Add(propertyDeclarationSyntax);
        }

        return validProperties;
    }

    /// <summary>
    /// Gets all fields as <see cref="FieldDeclarationSyntax"/> from a class declaration.
    /// </summary>
    /// <param name="classDeclarationSyntax"></param>
    /// <returns></returns>
    public static List<FieldDeclarationSyntax> GetAllFields(this ClassDeclarationSyntax classDeclarationSyntax,
        GeneratorExecutionContext context, bool recurseOverBaseClasses = false)
    {
        List<FieldDeclarationSyntax> fields = classDeclarationSyntax
            .DescendantNodes()
            .OfType<FieldDeclarationSyntax>()
            .ToList();

        if (recurseOverBaseClasses)
        {
            BaseTypeSyntax baseType = classDeclarationSyntax.BaseList?.Types.FirstOrDefault();
            if (baseType != null)
            {
                ITypeSymbol baseTypeSymbol = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree)
                    .GetTypeInfo(baseType.Type).Type;
                if (baseTypeSymbol != null)
                {
                    SyntaxReference baseTypeReference = baseTypeSymbol.DeclaringSyntaxReferences.FirstOrDefault();

                    if (baseTypeReference != null)
                        fields.AddRange(GetAllFields(baseTypeReference.GetSyntax() as ClassDeclarationSyntax,
                            context, true));
                }
            }
        }

        return fields;
    }

    /// <summary>
    /// Gets all fields from a class declaration and its base class if required.
    /// </summary>
    /// <param name="classDeclarationSyntax"></param>
    /// <param name="context"></param>
    /// <param name="recurseOverBaseClasses"></param>
    /// <returns></returns>
    public static List<IFieldSymbol> GetAllFieldSymbols(this ClassDeclarationSyntax classDeclarationSyntax,
        GeneratorExecutionContext context, bool recurseOverBaseClasses = false)
    {
        var semanticModel = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
        var classSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);
        var fields = new List<IFieldSymbol>();

        if (classSymbol != null)
        {
            fields.AddRange(classSymbol.GetMembers().OfType<IFieldSymbol>());

            while (recurseOverBaseClasses && classSymbol.BaseType != null)
            {
                classSymbol = classSymbol.BaseType;
                fields.AddRange(classSymbol.GetMembers().OfType<IFieldSymbol>());
            }
        }

        return fields;
    }

    /// <summary>
    /// Gets all members (fields, properties, and methods) from a class declaration and its base classes if required.
    /// </summary>
    /// <param name="classDeclarationSyntax">The class declaration syntax node.</param>
    /// <param name="context">The generator execution context.</param>
    /// <param name="recurseOverBaseClasses">Whether to recursively include members from base classes.</param>
    /// <param name="classFilter">A filter that ignores members if a predicate returns false. This does not stop base classes of ignored classes from being processed.</param>
    /// <returns>A list of all members (fields, properties, and methods) of the class and its base classes.</returns>
    public static List<ISymbol> GetAllMembers(this ClassDeclarationSyntax classDeclarationSyntax,
        GeneratorExecutionContext context, bool recurseOverBaseClasses = false, Predicate<INamedTypeSymbol> classFilter = null)
    {
        SemanticModel semanticModel = context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
        INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);
        List<ISymbol> members = new List<ISymbol>();

        if (classSymbol != null)
        {
            members.AddRange(classSymbol.GetMembers());

            while (recurseOverBaseClasses && classSymbol.BaseType != null)
            {
                classSymbol = classSymbol.BaseType;
                
                if(classFilter != null && !classFilter(classSymbol))
                    continue;
                
                members.AddRange(classSymbol.GetMembers());
            }
        }

        return members;
    }
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen.Utilities;

public static class UTCompilation
{
    /// <summary>
    /// Retrieves all class declarations within a given compilation.
    /// </summary>
    /// <param name="compilation">The compilation to search for class declarations.</param>
    /// <returns>An enumerable collection of ClassDeclarationSyntax objects representing the class declarations found.</returns>
    public static IEnumerable<ClassDeclarationSyntax> GetClasses(Compilation compilation)
    {
        IEnumerable<SyntaxNode> allNodes = compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
        IEnumerable<ClassDeclarationSyntax> allClasses = allNodes
            .Where(d => d.IsKind(SyntaxKind.ClassDeclaration))
            .OfType<ClassDeclarationSyntax>();
        return allClasses;
    }
}
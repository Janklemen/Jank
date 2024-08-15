using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen.Utilities;

public static class UTSyntaxTree
{
    /// <summary>
    /// Retrieves all class declaration syntax nodes from the given syntax tree.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree to search for class declarations. </param>
    /// <returns>
    /// An enumerable collection of class declaration syntax nodes found in the syntax tree.
    /// </returns>
    /// /
    public static IEnumerable<ClassDeclarationSyntax> GetClasses(this SyntaxTree syntaxTree)
    {
        IEnumerable<SyntaxNode> allNodes = syntaxTree.GetRoot().DescendantNodes();
        IEnumerable<ClassDeclarationSyntax> allClasses = allNodes
            .Where(d => d.IsKind(SyntaxKind.ClassDeclaration))
            .OfType<ClassDeclarationSyntax>();
        return allClasses;
    }
    
    /// <summary>
    /// Retrieves all the enum declarations in the given syntax tree.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree to analyze.</param>
    /// <returns>An enumerable collection of EnumDeclarationSyntax objects representing the enum declarations found in the syntax tree.</returns>
    public static IEnumerable<EnumDeclarationSyntax> GetEnums(this SyntaxTree syntaxTree)
        => syntaxTree.GetRoot().DescendantNodes().OfType<EnumDeclarationSyntax>();
}
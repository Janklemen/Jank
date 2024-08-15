using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen.Utilities;

public static class UTSyntaxNode
{
    public static string FullDeclarationIdentifier(this SyntaxNode node)
    {
        Stack<string> identifiers = new();

        while (node != null)
        {
            if (node is TypeDeclarationSyntax typ)
                identifiers.Push(typ.Identifier.ToString());

            if (node is BaseTypeDeclarationSyntax btyp)
                identifiers.Push(btyp.Identifier.ToString());

            if (node is NamespaceDeclarationSyntax nsp)
                identifiers.Push(nsp.Name.ToString());

            node = node.Parent;
        }

        List<string> order = new();
        while (identifiers.Count > 0)
            order.Add(identifiers.Pop());

        return string.Join(".", order);
    }

    public static string GetNearestNamespaceName(this SyntaxNode syntaxNode)
    {
        NamespaceDeclarationSyntax namespa = syntaxNode.Parent as NamespaceDeclarationSyntax;

        if (namespa == null)
        {
            if (syntaxNode.Parent == null)
                return string.Empty;

            return GetNearestNamespaceName(syntaxNode.Parent);
        }

        return namespa.Name.ToString();
    }
}
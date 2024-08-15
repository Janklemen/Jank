using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen.Utilities;

public static class UTAttributeListSyntax
{
    public static bool IsAttributeInList(this SyntaxList<AttributeListSyntax> listSyntax, string attributeName)
    {
        return listSyntax.Count(
            syntax => syntax.Attributes.Count(
                att =>
                {
                    if (att.Name is IdentifierNameSyntax identifierNameSyntax &&
                        identifierNameSyntax.Identifier.Text == attributeName)
                        return true;

                    if (att.Name is QualifiedNameSyntax qualifiedNameSyntax &&
                        qualifiedNameSyntax.ToString() == attributeName)
                        return true;
                    return false;
                }) > 0) > 0;
    }
}
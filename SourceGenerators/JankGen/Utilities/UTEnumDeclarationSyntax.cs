using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen.Utilities;

public static class UTEnumDeclarationSyntax
{
    public static bool IsWithAttribute(this EnumDeclarationSyntax enu, string attributeName)
    {
        return enu.AttributeLists.IsAttributeInList(attributeName);
    }
    
    public static IEnumerable<string> EnumMembers(this EnumDeclarationSyntax target)
        => target.Members.Select(m => m.Identifier.ToString());
}
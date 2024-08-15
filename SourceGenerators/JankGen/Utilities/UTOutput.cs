using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen.Utilities;

public static class UTOutput
{
    public static string ArrangeUsingRoslyn(string csCode)
    {
        var tree = CSharpSyntaxTree.ParseText(csCode);
        var root = tree.GetRoot().NormalizeWhitespace();
        var ret = root.ToFullString();
        return ret;
    }

    public static StringBuilder AggregateString(IEnumerable<string> strings)
    {
        return strings.Aggregate(new StringBuilder(), (s, syntax) => s.Append(syntax));
    }
    
    public static StringBuilder AggregateUsings(List<string> usings)
    {
        return usings.Distinct().Aggregate(new StringBuilder(), (builder, s) => builder.AppendLine($"using {s};"));
    }
    
    /// <summary>
    /// Recursively follows the syntax to calculate the required enclosure to correctly namespace the class
    /// </summary>
    /// <returns></returns>
    public static (string start, string end) GenerateEnclosingDeclarations(this SyntaxNode node)
    {
        Stack<string> start = new();
        Stack<string> end = new();

        if (node.Parent == null)
            return (string.Empty, string.Empty);

        SyntaxNode processing = node.Parent;

        while (processing != null)
        {
            if (processing is ClassDeclarationSyntax cls)
            {
                start.Push(cls.PartialClassDeclaration() + "{");
                end.Push("}");
            }

            if (processing is NamespaceDeclarationSyntax nps)
            {
                start.Push(NamespaceDeclaration(nps) + "{");
                end.Push("}");
            }

            processing = processing.Parent;
        }

        StringBuilder startSb = new();
        StringBuilder endSb = new();

        while (start.Count > 0)
            startSb.AppendLine(start.Pop());

        while (end.Count > 0)
            endSb.AppendLine(end.Pop());

        return (startSb.ToString(), endSb.ToString());
    }
    
    public static string PartialClassDeclaration(this ClassDeclarationSyntax target)
        => $"public partial class {target.Identifier.ToString()}";

    public static string NamespaceDeclaration(this NamespaceDeclarationSyntax target)
        => $"namespace {target.Name.ToString()}";

    public static string StaticPartialClass(this string name)
        => $"public static partial class {name}";
    
    public static string Decapitialize(this string s) => char.ToLower(s[0]) + s.Substring(1);
}
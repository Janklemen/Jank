using System.Collections.Generic;
using System.Linq;
using System.Text;
using JankGen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen
{
    [Generator]
    public class JankSerializableGenerator : AJankGenerator
    {
        const string cAttributeName = "JankSerializable";

        public override void ExecuteClassGenerator(GeneratorExecutionContext context,
            Compilation compilation, SemanticModel model,
            ClassDeclarationSyntax classDeclarationSyntax)
        {
            if (!classDeclarationSyntax.IsWithAttribute(cAttributeName))
                return;

            // generatorExecutionContext.AddSource(
            //     "test", 
            //     "test"
            //     );
            //
            // return;

            NamespaceDeclarationSyntax namespa = classDeclarationSyntax.Parent as NamespaceDeclarationSyntax;

            string namespaceName = namespa.Name.ToString();

            string className = classDeclarationSyntax.Identifier.ToString();

            List<FieldDeclarationSyntax> validFields = classDeclarationSyntax.GetAllFields(context).GetValidFields(
                f => f.AttributeLists.IsAttributeInList("SerializeField")
            );

            List<string> usings = new();
            usings.Add("System");
            usings.Add("System.Linq");
            usings.Add("System.Threading");
            usings.Add("System.Text");
            usings.Add("System.Collections.Generic");
            usings.Add("UnityEngine");
            usings.Add("Jank.Utilities");
            usings.Add("Cysharp.Threading.Tasks");
            usings.Add("Jank.Serialization");

            List<string> serializeStings =
                validFields.Select(f => CreateSerialize(f, model)).ToList();

            List<string> deserializeStrings =
                validFields.Select(f => CreateDeserialize(f, model)).ToList();

            string classGeneratedSourceCode = $@"
{UTOutput.AggregateUsings(usings)}

namespace {namespaceName} 
{{
    public partial class {className} : IJankSerializable
    {{
        public string Serialize()
        {{
            StringBuilder sb = new();
            Serialize(sb);
            return sb.ToString();
        }}

        public void Serialize(StringBuilder sb)
        {{
            {UTOutput.AggregateString(serializeStings)}
        }}

        public void Deserialize(string data)
        {{
            Queue<string> lines = new Queue<string>(data.Split(Environment.NewLine));
            Deserialize(lines);
        }}

        public void Deserialize(Queue<string> lines)
        {{
            {UTOutput.AggregateString(deserializeStrings)}
        }}
    }}
}}
";
            context.AddSource($"{classDeclarationSyntax.Identifier}.serializer.g",
                UTOutput.ArrangeUsingRoslyn(classGeneratedSourceCode));
        }

        public override void ExecuteEnumGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model, EnumDeclarationSyntax enumDeclarationSyntax)
        {
        }

        public override void ExecutePostGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model)
        {
        }

        public string CreateSerialize(FieldDeclarationSyntax syntax, SemanticModel model)
        {
            TypeInfo info = model.GetTypeInfo(syntax.Declaration.Type);
            string t = syntax.Declaration.Type.GetText().ToString().Trim();

            if (info.Type.BaseType is {Name: "ScriptableObject"})
                return $"{syntax.GetFieldDeclarationIdentifier()}.Serialize(sb);";

            if (info.Type is {TypeKind: TypeKind.Array})
                return
                    $"sb.AppendLine(string.Join(',', {syntax.GetFieldDeclarationIdentifier()}.Select(s => s.ToString())));";

            return $"sb.AppendLine({syntax.GetFieldDeclarationIdentifier()}.ToString());";
        }

        public string CreateDeserialize(FieldDeclarationSyntax syntax, SemanticModel model)
        {
            TypeInfo info = model.GetTypeInfo(syntax.Declaration.Type);
            string t = syntax.Declaration.Type.GetText().ToString().Trim();

            SyntaxToken identifier = syntax.GetFieldDeclarationIdentifier();
            if (info.Type.BaseType is {Name: "ScriptableObject"} &&
                info.Type.GetAttributes().Any(a => a.ToString().Contains(cAttributeName)))
            {
                StringBuilder sb = new();
                sb.AppendLine(
                    $"{identifier} = {identifier} == default ? ScriptableObject.CreateInstance<{info.Type}>() : {identifier};");
                sb.AppendLine($"{identifier}.Deserialize(lines);");
                return sb.ToString();
            }

            if (info.Type is {TypeKind: TypeKind.Enum})
                return $"{identifier} = Enum.Parse<{info.Type}>(lines.Dequeue());";

            if (info.Type.ToString() == "string")
                return $"{identifier} = lines.Dequeue();";

            if (info.Type is {TypeKind: TypeKind.Array})
                return
                    $"{identifier} = lines.Dequeue().Split(\",\").Select(v => {info.Type.ToString().Substring(0, info.Type.ToString().IndexOf('['))}.Parse(v)).ToArray();";

            return $"{identifier} = {info.Type}.Parse(lines.Dequeue());";
        }
    }
}
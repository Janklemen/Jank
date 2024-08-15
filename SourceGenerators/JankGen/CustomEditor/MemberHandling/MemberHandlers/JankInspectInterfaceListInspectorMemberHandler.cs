using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using Microsoft.CodeAnalysis;

namespace Jank.Inspector.CustomEditorGenerator
{
    public class JankInspectInterfaceListInspectorMemberHandler : AMemberHandler
    {
        public override bool CanHandleMember(ISymbol member)
        {
            if (member.GetAttributes().All(a => a.AttributeClass.Name != "JankInspectAttribute"))
                return false;

            if (member is IFieldSymbol ifs && ifs.Type is INamedTypeSymbol namedType1 &&
                namedType1.ConstructedFrom.ToString() == "System.Collections.Generic.List<T>" &&
                namedType1.TypeArguments[0].TypeKind == TypeKind.Interface)
                return true;

            if (member is IPropertySymbol ips && ips.Type is INamedTypeSymbol namedType2 &&
                namedType2.ConstructedFrom.ToString() == "System.Collections.Generic.List<T>" &&
                namedType2.TypeArguments[0].TypeKind == TypeKind.Interface)
                return true;

            return false;
        }

        public override string Generate(ISymbol info, IMemberHandler.GenerationEvent evt)
        {
            string backingObject = $"_{info.Name}__backingObject";

            string typeName = "NotRight";
            
            if (info is IFieldSymbol ifs && ifs.Type is INamedTypeSymbol namedType1)
                typeName = namedType1.TypeArguments[0].ToDisplayString();
            
            if (info is IPropertySymbol ips && ips.Type is INamedTypeSymbol namedType2)
                typeName = namedType2.TypeArguments[0].ToDisplayString();
            
            switch (evt)
            {
                case IMemberHandler.GenerationEvent.RuntimeMembers:
                    return $"[SerializeField, JankInspectIgnore] List<UnityEngine.Object> {backingObject};";
                case IMemberHandler.GenerationEvent.RuntimeOnAfterDeserialize:
                    return $@"
                    // On Deserialization, set the interface
                    {info.Name} = {backingObject}.Select(o => o as {typeName}).ToList();";
            }

            return string.Empty;
        }
    }
}
using System;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Jank.Inspector.CustomEditorGenerator
{
    public class JankInspectInterfaceInspectorMemberHandler : AMemberHandler
    {
        public override bool CanHandleMember(ISymbol member)
        {
            if (member.GetAttributes().All(a => a.AttributeClass.Name != "JankInspectAttribute"))
                return false;

            if (member is IFieldSymbol ifs && ifs.Type.TypeKind == TypeKind.Interface)
                return true;
            
            if (member is IPropertySymbol ips && ips.Type.TypeKind == TypeKind.Interface)
                return true;
            
            return false;
        }

        public override string Generate(ISymbol info, IMemberHandler.GenerationEvent evt)
        {
            string backingObject = $"_{info.Name}__backingObject";

            string typeName = "NotRight";
            
            if (info is IFieldSymbol ifs)
                typeName = ifs.Type.ToDisplayString();
            
            if (info is IPropertySymbol ips)
                typeName = ips.Type.ToDisplayString();
            
            switch (evt)
            {
                case IMemberHandler.GenerationEvent.RuntimeMembers:
                    return $"[SerializeField, JankInspectIgnore] UnityEngine.Object {backingObject};";
                case IMemberHandler.GenerationEvent.RuntimeOnAfterDeserialize:
                    return $@"
                    // On Deserialization, set the interface
                    {info.Name} = {backingObject} as {typeName};";
            }

            return string.Empty;
        }
    }
}
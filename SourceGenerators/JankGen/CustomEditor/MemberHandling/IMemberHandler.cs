using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Jank.Inspector.CustomEditorGenerator
{
    public interface IMemberHandler
    {
        public enum GenerationEvent
        {
            RuntimeMembers,
            RuntimeOnAfterDeserialize
        }
        
        public bool CanHandleMember(ISymbol member);
        public string Generate(ISymbol info, GenerationEvent evt);
    }
}
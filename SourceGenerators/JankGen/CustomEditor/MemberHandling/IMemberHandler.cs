using Microsoft.CodeAnalysis;

namespace JankGen.CustomEditor.MemberHandling
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
using Microsoft.CodeAnalysis;

namespace JankGen.CustomEditor.MemberHandling.MemberHandlers
{
    public abstract class AMemberHandler : IMemberHandler
    {
        public abstract bool CanHandleMember(ISymbol member);
        public abstract string Generate(ISymbol info, IMemberHandler.GenerationEvent evt);
    }
}
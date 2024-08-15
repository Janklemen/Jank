using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Jank.Inspector.CustomEditorGenerator
{
    public abstract class AMemberHandler : IMemberHandler
    {
        public abstract bool CanHandleMember(ISymbol member);
        public abstract string Generate(ISymbol info, IMemberHandler.GenerationEvent evt);
    }
}
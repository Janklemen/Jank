using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Jank.Inspector.CustomEditorGenerator
{
    public static class UTMemberHandler
    {
        static IMemberHandler[] _generators = {
            new JankInspectInterfaceInspectorMemberHandler(),
            new JankInspectInterfaceListInspectorMemberHandler(),
        };

        public static bool TryGetMemberHandler(ISymbol member, out IMemberHandler handler)
        {
            foreach (IMemberHandler memberHandler in _generators)
            {
                handler = memberHandler;

                if (memberHandler.CanHandleMember(member))
                    return true;
            }

            handler = default;
            return false;
        }
    }
}
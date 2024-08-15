#if UNITY_EDITOR
using System.Reflection;

namespace Jank.Inspector.CustomEditorGenerator
{
    public static class UTMemberHandler
    {
        static IMemberHandler[] _generators = {
            new SerializedFieldInspectorMemberHandler(),
            new JankInspectInterfaceInspectorMemberHandler(),
            new JankInspectInterfaceListInspectorMemberHandler(),
            new JankInspectObjectInspectorMemberHandler(),
            new ButtonInspectorMemberHandler()
        };

        public static bool TryGetMemberHandler(MemberInfo member, out IMemberHandler handler)
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
#endif
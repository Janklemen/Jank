using System;

namespace Jank.Inspector.CustomEditorGenerator
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class JankInspectAttribute : Attribute
    {
        
    }
}
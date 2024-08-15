using System;

namespace Jank.Objects
{
    /// <summary>
    /// Used by the JankInjectableGenerator to indicate that a field needs injecting by label
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class JankInjectLabeledAttribute : Attribute
    {
        public JankInjectLabeledAttribute(string label)
        {
        }
    }
}
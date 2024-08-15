using System;

namespace Jank.Objects
{
    /// <summary>
    /// Used by the JankInjectableGenerator to indicate that a field need injecting by type
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class JankInjectAttribute : Attribute
    {

    }
}
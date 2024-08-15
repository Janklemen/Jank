using System;

namespace Jank.Props
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JankPropMenuItemAttribute : Attribute
    {
        public string[] OtherProps { get; }

        public JankPropMenuItemAttribute(params string[] otherProps)
        {
            OtherProps = otherProps;
        }
    }
}
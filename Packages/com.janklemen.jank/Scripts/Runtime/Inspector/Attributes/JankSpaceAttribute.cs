using System;

namespace Jank.Inspector.CustomEditorGenerator
{
    public class JankSpaceAttribute : Attribute
    {
        public float Space;

        public JankSpaceAttribute(float space = 10)
        {
            Space = space;
        }
    }
}
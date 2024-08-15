using System;

namespace Jank.Inspector.CustomEditorGenerator
{
    public class JankHeaderAttribute : Attribute
    {
        public string Header;

        public JankHeaderAttribute(string header)
        {
            Header = header;
        }
    }
}
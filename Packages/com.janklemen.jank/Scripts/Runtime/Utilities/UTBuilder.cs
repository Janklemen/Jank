using System;

namespace Jank.Utilities
{
    public static class UtBuilder
    {
        public static void ThrowIfNull<T>(T resource, string name)
        {
            if (resource == null)
                throw new Exception($"Cannot build without setting resource: {name}");
        }
    }
}
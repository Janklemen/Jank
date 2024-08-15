namespace Jank.Utilities
{
    public static class UTStandardStringFormats
    {
        public enum ETupleFormat
        {
            Index
        }

        public static string JankFormat<T1, T2>(this (T1 x, T2 y) tuple, ETupleFormat format)
        {
            switch (format)
            {
                case ETupleFormat.Index: return $"[{tuple.x}, {tuple.y}]";
            }

            return tuple.ToString();
        }
    }
}
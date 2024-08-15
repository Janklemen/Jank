namespace Jank.Primatives.Object
{
    public static class UtObject
    {
        public static bool TryAs<T>(this object ob, out T val)
        {
            if (!(ob is T))
            {
                val = default;
                return false;
            }

            val = (T)ob;
            return true;
        }
    }
}
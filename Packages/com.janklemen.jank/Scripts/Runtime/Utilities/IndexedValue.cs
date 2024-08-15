namespace Jank.Utilities
{
    public struct IndexedValue<T>
    {
        public T Value;
        public int Index;

        public IndexedValue(T value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}
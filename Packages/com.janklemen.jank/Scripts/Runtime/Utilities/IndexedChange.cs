namespace Jank.Utilities
{
    public struct IndexedChange<T>
    {
        public T OldValue;
        public T NewValue;
        public int Index;

        public IndexedChange(T oldValue, T newValue, int index)
        {
            OldValue = oldValue;
            NewValue = newValue;
            Index = index;
        }
    }
}
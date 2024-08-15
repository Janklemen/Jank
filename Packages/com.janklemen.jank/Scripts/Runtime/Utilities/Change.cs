namespace Jank.Utilities
{
    public struct Change<T>
    {
        public T OldValue;
        public T NewValue;

        public Change(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
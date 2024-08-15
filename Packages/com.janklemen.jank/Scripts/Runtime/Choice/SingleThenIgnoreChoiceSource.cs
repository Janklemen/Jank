namespace Jank.Choice
{
    public class SingleThenIgnoreChoiceSource<T> : AChoiceSource<T>
    {
        /// <inheritdoc />
        public override void Choose(T choice)
        {
            if (IsChosen)
                return;

            Choice = choice;
            IsChosen = true;
        }
    }
}
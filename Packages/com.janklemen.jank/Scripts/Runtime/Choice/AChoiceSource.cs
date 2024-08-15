using Cysharp.Threading.Tasks;

namespace Jank.Choice
{
    /// <summary>
    /// <see cref="AChoiceSource{T}"/> encapsulate simple long poll async choice logic. They are meant to be accessible
    /// to a consumer and a chooser. The consumer waits for the chooser to <see cref="Choose"/>. Once called, the long
    /// poll detects the choice and returns to the consumer. Only one consumer can claim the
    /// <see cref="AChoiceSource{T}"/> at a time.
    /// </summary>
    /// <typeparam name="T">The class the describes the data structure used to represent the users choice</typeparam>
    /// <remarks>
    /// <para> This asks as a bridge between sync and async code. The <see cref="AwaitChoice"/> function performs a
    /// long poll on the Update loop, which allows sync code to call <see cref="Choose"/> which returns the
    /// <see cref="Choice"/> async.</para>
    /// </remarks>
    public abstract class AChoiceSource<T> {
        public bool IsChosen { get; protected set; }
        public T Choice { get; protected set; }
        
        /// <summary>
        /// Determines the behaviour of choosing. Different behaviour will be desired in difference situations. Consider
        /// a <see cref="AChoiceSource{T}"/>> that allows the choice to keep changing before a consumer consumes the
        /// choice, vs a <see cref="AChoiceSource{T}"/> that only allows a single choose then ignores everything after.
        /// </summary>
        /// <param name="choice"></param>
        public abstract void Choose(T choice);
        
        /// <summary>
        /// Allows async code to wait for choice to be made by delaying on the update loop using
        /// <see cref="UniTask.DelayFrame"/>
        /// </summary>
        /// <param name="">Cancellation token for stopping the await</param>
        /// <returns>Task that results in A choice</returns>
        public async UniTask<T> AwaitChoice()
        {
            while (!IsChosen)
                await UniTask.DelayFrame(1, PlayerLoopTiming.Update);

            return Choice;
        }

        public void Reset()
        {
            IsChosen = false;
            Choice = default;
        }
    }
}
using System.Threading;

namespace Jank.App
{
    /// <summary>
    /// Contains important global application signals that are interpreted by the <see cref="AJankContainer"/>
    /// </summary>
    public class AppSignals
    {
        /// <summary>
        /// The token used to quit the game. 
        /// </summary>
        public CancellationTokenSource Quit { get; private set; }
        
        public AppSignals(CancellationTokenSource quit)
        {
            Quit = quit;
        }

        public static implicit operator CancellationToken(AppSignals signals)
            => signals.Quit.Token;
    }
}
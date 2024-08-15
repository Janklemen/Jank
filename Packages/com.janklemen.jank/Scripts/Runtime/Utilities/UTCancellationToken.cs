using System.Threading;

namespace Jank.Utilities
{
    public static class UTCancellationTokenSource
    {
        public static void CancelAndRenew(ref CancellationTokenSource src)
        {
            if(src != null)
                src.Cancel();

            src = new();
        }
    }
}
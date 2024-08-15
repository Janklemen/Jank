using System;
using Jank.Feedback;
using Jank.Observables.MouseEvents;
using UnityEngine;

namespace Jank.Debugging.LogFactories
{
    public static class UTLogFactory
    {
        public static Action<EStandardFeedbackEvent> MouseEventLogger(string lead)
        {
            return e => Debug.Log($"[{lead}] {e.ToString()}");
        }
    }
}
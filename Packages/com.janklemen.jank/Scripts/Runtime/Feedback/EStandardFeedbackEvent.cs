using System;

namespace Jank.Feedback
{
    [Flags]
    public enum EStandardFeedbackEvent
    {
        Selectable = 1,
        Selected = 2,
        Enter = 4,
        Down = 8,
        Up = 16,
        Exit = 32,
        Appearing = 64,
        Disappearing = 128,
        Moving = 256,
        BeingAffected = 512,
        Affecting = 1024
    }
}
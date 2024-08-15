using System;

namespace Jank.States
{
    /// <summary>
    /// Used with the JankMachineStatesGenerator to mark an enum as a representation of a state machines states.
    /// An abstract MonoBehaviour will be generated called A{EnumName}Machine which can be inherited to implement the
    /// state logic.
    /// </summary>
    /// <remarks>
    /// The enum must have one entry called Terminate.
    ///
    /// A corresponding class {EnumName}State is required. This class is used by the state machine as the state object
    /// being modified.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Enum)]
    public class JankRulesAttribute : Attribute
    {
        
    }
}
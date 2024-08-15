using System;

namespace Jank.States
{
    /// <summary>
    /// Used in conjunction with the JankStateGenerator, marking a partial class with this attribute generates
    /// functions for performing important state actions for each field present in the class of any visibility level.
    /// </summary>
    /// <remarks>
    /// <para>The OnSignal and Signal functions are provided which allow listeners to subscribe to changes of the target
    /// field</para>
    /// <para>A Bind function is generated that automatically binds an action to the OnSignal event, and calls
    /// Signal. The Set function also calls Signal after the value has been set.</para>
    /// <para>A get function is generated.</para>
    /// <para>Special object types are handled. Array types are provided with the above functions for each element in
    /// the array</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class JankStateAttribute : Attribute
    {
        
    }
}
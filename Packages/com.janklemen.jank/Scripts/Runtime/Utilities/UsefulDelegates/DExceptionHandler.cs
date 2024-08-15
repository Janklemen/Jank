using System;

namespace Jank.Utilities.UsefulDelegates
{
    /// <summary>
    /// Takes an Exception as a parameter and handles it
    /// </summary>
    /// <param name="e">Exception object</param>
    public delegate void DExceptionHandler(Exception e);
}
﻿using System.Collections;
using System.Collections.Generic;

namespace Jank.DotNet.Enumerator
{
    /// <summary>
    /// Exists simply to wrap enumerator in enumerable for foreach loop easy
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicEnumerable<T> : IEnumerable<T>
    {
        IEnumerator<T> _enumerator;

        #region API
        /// <summary>
        /// Create a basic Enumerable
        /// </summary>
        /// <param name="enumerator">Enumerator</param>
        public BasicEnumerable(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        /// <summary>
        /// Get the enumerator from the enumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _enumerator;
        }
        #endregion

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _enumerator;
        }
    }
}

using System;

namespace Jank.Patterns.Singleton
{
    /// <summary>
    /// Implemntation of Singleton as an abstract class blinly folowing 
    /// https://csharpindepth.com/articles/singleton version six until I 
    /// have time to look closer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ASingleton<T>
        where T : new()
    {
        static readonly Lazy<T> k_Lazy = new(() => new T());

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static T Instance { get { return k_Lazy.Value; } }

        ASingleton()
        {
        }
    }
}

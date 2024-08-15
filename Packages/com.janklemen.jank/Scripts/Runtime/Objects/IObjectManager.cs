using System;
using System.Collections.Generic;

namespace Jank.Objects
{
    public interface IObjectManager
    {
        IReadOnlyDictionary<Type, object> Singles { get; }
        IReadOnlyDictionary<string, object> Labeled { get; }
     
        /// <summary>
        /// Register a dependency. It can be injected in any class that contains a field annotated with the
        /// <see cref="JankInjectAttribute"/>
        /// </summary>
        /// <param name="dependency">The instance to add as a dependency</param>
        /// <typeparam name="T">The type it will be registered as. It must match the type of the <see cref="JankInjectAttribute"/> field for injection to be successful</typeparam>
        void RegisterSingle<T>(T dependency);

        /// <summary>
        /// Register a dependency that can be looked up by label rather than type. It can be injected in any class that
        /// contains a field annotated with <see cref="JankInjectLabeledAttribute"/>
        /// </summary>
        /// <param name="label">label of the dependency. It must match the label provided to the <see cref="JankInjectLabeledAttribute"/> for injection to be successful</param>
        /// <param name="dependency">The instance to add to the labeled dependency list</param>
        void RegisterLabeled(string label, object dependency);

        /// <summary>
        /// If the object is <see cref="IJankInjectable"/>, runs the objects injection code and resolves dependencies.
        /// Other objects will be returned as is.
        /// </summary>
        /// <param name="obj">An instance of the object provided</param>
        /// <typeparam name="T">The object instance to process</typeparam>
        /// <returns>The object after processing</returns>
        T ProcessObject<T>(T obj);
    }
}
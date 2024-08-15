using System;
using System.Collections.Generic;
using System.Linq;

namespace Jank.Utilities
{
    public static class UTTypes
    {
        public static IEnumerable<(Type, TAttribute)> WithAttribute<TAttribute>(this IEnumerable<Type> types)
        {
            return types
                .Select(type => (type, type.GetCustomAttributes(true)))
                .SelectWhere(pair => pair.Item2.Any(ob => ob.IsOrSubclassOf<TAttribute>()))
                .Select(pair => (pair.Item1, (TAttribute) pair.Item2.First(ob => ob.IsOrSubclassOf<TAttribute>())));
        }
        
        public static IEnumerable<(Type, Attribute)> WithAttributesOr<TAttribute1, TAttribute2>(this IEnumerable<Type> types)
            where TAttribute1 : Attribute
            where TAttribute2 : Attribute
        {
            return types
                .Select(type => (type, type.GetCustomAttributes(true)))
                .SelectWhere(pair => pair.Item2.Any(ob => ob.IsOrSubclassOf<TAttribute1>() || ob.IsOrSubclassOf<TAttribute2>()))
                .Select(pair => (pair.Item1, (Attribute) pair.Item2.First(ob => ob.IsOrSubclassOf<TAttribute1>() || ob.IsOrSubclassOf<TAttribute2>())));
        }

        public static bool IsAssignableFrom<T>(this object target)
            => target.GetType().IsAssignableFrom(typeof(T));
        
        public static bool IsSubclassOf<T>(this object target)
            => target.GetType().IsSubclassOf(typeof(T));
        
        public static bool IsInstanceOf<T>(this object target)
            => target.GetType().IsInstanceOfType(typeof(T));
        
        public static bool IsOrSubclassOf<T>(this object target)
            => target is T || target.IsSubclassOf<T>() || target.IsInstanceOf<T>();

        /// <summary>
        /// Reflection can be confusing sometimes. My personal definition of Is, when I asked the question is the
        /// following: In the situation where you ask is a of type b? I want to know would type b = a work. This
        /// means that interfaces and abstract classes will be detected. Also type a = a. This is simple is
        /// assignable from expression, but logically gets reversed often. This Is function is a simple way to
        /// call IsAssignableFrom in the right order.  
        /// </summary>
        public static bool Is(this object target, Type t)
            => t.IsAssignableFrom(target.GetType());

        public static IEnumerable<Type> GetFullHierarchy(Type t)
        {
            List<Type> collection = new();

            Queue<Type> processing = new();
            processing.Enqueue(t);

            while (processing.Count > 0)
            {
                Type next = processing.Dequeue();
                
                if(!collection.Contains(next))
                    collection.Add(next);
                
                if(next.BaseType != null)
                    processing.Enqueue(next.BaseType);
                
                foreach (Type inter in next.GetInterfaces())
                    processing.Enqueue(inter);
            }

            return collection;
        }
    }
}
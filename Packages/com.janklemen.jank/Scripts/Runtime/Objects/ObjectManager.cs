using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Jank.Utilities;
using UnityEngine;

namespace Jank.Objects
{
    /// <summary>
    /// The object manager exists to manage the destruction and instantiation of objects. 
    /// </summary>
    /// <remarks>
    /// <para>There should only be one <see cref="ObjectManager"/> per container.</para>
    /// 
    /// <para>Many times, objects can be recycled instead of being added and removed from memory. This is what the object
    /// manager does, instead of calling Destroy on an Object causing the engine to
    /// remove it from memory this object can simply be disabled and enabled when needed again. This skips removing it
    /// from memory.</para>
    ///
    /// <para>The ability of the <see cref="ObjectManager"/> to perform dependency injection depends on the
    /// JankInjectableGenerator found in JankleSourceGenerators.</para>
    ///
    /// <para>There are a few important terms to know:</para>
    /// <list type="bullet">
    /// <item><description>Register: Provide an existing instance of a type and add it to a dependency list to be
    /// injected into processed objects</description></item>
    /// <item><description>Process: The act of taking an instance of an object and injecting dependencies</description></item>
    /// <item><description>Supply: Get the object manager to create an instance of an object for you, process it, and track its lifecycle</description></item>
    /// <item><description>Return: Give back a previously supplied object so that it can be recycled</description></item>
    /// <item><description>Tag: Give the instance a tag and register it. Useful for serialization.</description></item>
    /// <item><description>Untag: Remove the instance from the tagged list. Normally used when a tagged instance is out of the game and it's stats are no longer relevant.</description></item>
    /// </list>
    /// </remarks>
    public class ObjectManager : IObjectManager, IObjectPool
    {
        readonly Dictionary<Guid, object> _tagged = new();
        readonly Dictionary<Type, object> _singles = new();
        readonly Dictionary<string, object> _labeled = new();

        IObjectPool _pool = ObjectPool.Instance;

        public IReadOnlyDictionary<Type, object> Singles => _singles;
        public IReadOnlyDictionary<string, object> Labeled => _labeled;

        public ObjectManager(Transform parent = null)
        {
            RegisterSingle((IObjectManager)this);
            RegisterSingle((IObjectPool)this);
        }

        /// <summary>
        /// Register a dependency. It can be injected in any class that contains a field annotated with the
        /// <see cref="JankInjectAttribute"/>
        /// </summary>
        /// <param name="dependency">The instance to add as a dependency</param>
        /// <typeparam name="T">The type it will be registered as. It must match the type of the <see cref="JankInjectAttribute"/> field for injection to be successful</typeparam>
        public void RegisterSingle<T>(T dependency)
        {
            foreach (Type type in UTTypes.GetFullHierarchy(dependency.GetType()))
                _singles[type] = dependency;
            ReprocessAllObjects();
        }

        void ReprocessAllObjects()
        {
            HashSet<object> allObjects = new();

            foreach (object value in _singles.Values)
                allObjects.Add(value);

            foreach (object value in _tagged.Values)
                allObjects.Add(value);

            foreach (object value in _labeled.Values)
                allObjects.Add(value);

            UTIEnumerable.ForEach(allObjects, o => ProcessObject(o));
        }

        /// <summary>
        /// Register a dependency that can be looked up by label rather than type. It can be injected in any class that
        /// contains a field annotated with <see cref="JankInjectLabeledAttribute"/>
        /// </summary>
        /// <param name="label">label of the dependency. It must match the label provided to the <see cref="JankInjectLabeledAttribute"/> for injection to be successful</param>
        /// <param name="dependency">The instance to add to the labeled dependency list</param>
        public void RegisterLabeled(string label, object dependency)
        {
            _labeled.Add(label, dependency);
            ReprocessAllObjects();
        }

        /// <summary>
        /// If the object is <see cref="IJankInjectable"/>, runs the objects injection code and resolves dependencies.
        /// Other objects will be returned as is.
        /// </summary>
        /// <param name="obj">An instance of the object provided</param>
        /// <typeparam name="T">The object instance to process</typeparam>
        /// <returns>The object after processing</returns>
        public T ProcessObject<T>(T obj)
        {
            if (obj == null)
                return obj;
            
            if (obj is IJankInjectable injectable)
                injectable.Inject(this);
            else
            {
                foreach (MemberInfo memberInfo in obj.GetType().GetMembers(BindingFlags.Static
                                                                           | BindingFlags.Public
                                                                           | BindingFlags.NonPublic
                                                                           | BindingFlags.Instance))
                {
                    ProcessMember(obj, memberInfo);
                }
            }

            return obj;
        }

        public void ProcessMember(object obj, MemberInfo memberInfo)
        {
            if (obj == null)
                return;

            try
            {
                if (memberInfo is FieldInfo fi)
                {
                    if (fi.GetValue(obj) != null && fi.GetValue(obj) is IJankInjectable fiv)
                        fiv.Inject(this);
                    else
                    {
                        ProcessMember(fi.GetValue(obj), fi.FieldType);
                    }
                }
                else if (memberInfo is PropertyInfo pi)
                {
                    if (pi.GetValue(obj) != null && pi.GetValue(obj) is IJankInjectable piv)
                        piv.Inject(this);
                    else
                    {
                        ProcessMember(pi.GetValue(obj), pi.PropertyType);
                    }
                }
            }
            catch (Exception)
            {
                // Ignore anything that throws an exception on GetValue
            }
        }

        public void ProcessGameObject(GameObject obj)
        {
            obj.BFS(g =>
            {
                foreach (MonoBehaviour monoBehaviour in g.GetComponents<MonoBehaviour>())
                    ProcessObject(monoBehaviour);
            });
        }

        public async UniTask<T> Supply<T>() where T : IManageableObject, new()
        {
            T obj = await _pool.Supply<T>();
            ProcessObject(obj);
            return obj;
        }

        public async UniTask<GameObject> Supply(GameObject prefab)
        {
            GameObject instance = await _pool.Supply(prefab);
            ProcessGameObject(instance);
            return instance;
        }

        public async UniTask<GameObject> Prewarm(GameObject prefab)
            => await _pool.Prewarm(prefab);

        public void Return<T>(T obj) where T : IManageableObject, new()
            => _pool.Return(obj);

        public void Return(GameObject obj)
            => _pool.Return(obj);
    }
}
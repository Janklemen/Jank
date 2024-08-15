using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Jank.Utilities.MainThreadDispatch
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        readonly ConcurrentQueue<Action> _actions = new(); 
        
        void Update()
        {
            while (_actions.TryDequeue(out Action action))
                action();
        }

        public void Enqueue(Action action) => _actions.Enqueue(action);
    }
}

using System;
using System.Collections.Generic;

namespace App.Simples
{
    public sealed class SEvent
    {
        private readonly List<Action> _listeners = new();

        public void AddListener(Action action)
            => _listeners.Add(action);
        public void RemoveListener(Action action)
            => _listeners.Remove(action);
        public void ClearListeners()
            => _listeners.Clear();
        public void Invoke()
        {
            Action[] actions = _listeners.ToArray();
            foreach (Action action in actions)
                action?.Invoke();
        }
    }
    public sealed class SEvent<T>
    {
        private readonly List<Action<T>> _listeners = new();

        public void AddListener(Action<T> action)
            => _listeners.Add(action);
        public void RemoveListener(Action<T> action)
            => _listeners.Remove(action);
        public void ClearListeners()
            => _listeners.Clear();
        public void Invoke(T t)
        {
            Action<T>[] actions = _listeners.ToArray();
            foreach (Action<T> action in actions)
                action?.Invoke(t);
        }
    }
    public sealed class SEvent<T1, T2>
    {
        private readonly List<Action<T1, T2>> _listeners = new();

        public void AddListener(Action<T1, T2> action)
            => _listeners.Add(action);
        public void RemoveListener(Action<T1, T2> action)
            => _listeners.Remove(action);
        public void ClearListeners()
            => _listeners.Clear();
        public void Invoke(T1 t1, T2 t2)
        {
            Action<T1, T2>[] actions = _listeners.ToArray();
            foreach (Action<T1, T2> action in actions)
                action?.Invoke(t1, t2);
        }
    }
    public sealed class SEvent<T1, T2, T3>
    {
        private readonly List<Action<T1, T2, T3>> _listeners = new();

        public void AddListener(Action<T1, T2, T3> action)
            => _listeners.Add(action);
        public void RemoveListener(Action<T1, T2, T3> action)
            => _listeners.Remove(action);
        public void ClearListeners()
            => _listeners.Clear();
        public void Invoke(T1 t1, T2 t2, T3 t3)
        {
            Action<T1, T2, T3>[] actions = _listeners.ToArray();
            foreach (Action<T1, T2, T3> action in actions)
                action?.Invoke(t1, t2, t3);
        }
    }
}
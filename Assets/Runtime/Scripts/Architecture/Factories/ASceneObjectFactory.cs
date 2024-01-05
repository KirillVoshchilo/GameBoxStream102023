using App.Simples;
using UnityEngine;

namespace App.Architecture.Factories
{
    public abstract class ASceneObjectFactory<T>
    {
        private readonly SEvent<T> _onCreated = new();
        private readonly SEvent<T> _onRemoved = new();

        protected Transform _parent;

        public Transform Parent { set => _parent = value; }
        public SEvent<T> OnCreated => _onCreated;
        public SEvent<T> OnRemoved => _onRemoved;

        public abstract T Create();
        public abstract void Remove(T obj);
    }
}
using UnityEngine;

namespace App.Architecture.AppData
{
    public abstract class AScriptableFactory : ScriptableObject
    {
        private Transform _parent;

        public Transform Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public abstract void Create();
        public abstract void Remove(GameObject obj);
    }
}
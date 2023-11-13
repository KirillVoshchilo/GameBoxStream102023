using App.Architecture.AppData;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace App.Logic
{
    public sealed class LevelStorage : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private GameObject[] _autoInjectObjects;

        private readonly HashSet<IDestructable> _destructables = new();


        public Transform PlayerTransform
            => _playerTransform;

        public void Construct(LifetimeScope lifeTimeScope)
            => AutoInjectAll(lifeTimeScope);
        public void Destruct()
        {
            foreach (IDestructable destructable in _destructables)
                destructable.Destruct();
        }

        private void AutoInjectAll(LifetimeScope lifeTimeScope)
        {
            if (_autoInjectObjects == null)
                return;
            foreach (GameObject target in _autoInjectObjects)
            {
                if (target != null)
                {
                    _destructables.UnionWith(target.GetComponents<IDestructable>());
                    _destructables.UnionWith(target.GetComponentsInChildren<IDestructable>());
                    lifeTimeScope.Container.InjectGameObject(target);
                }
            }
        }
    }
}
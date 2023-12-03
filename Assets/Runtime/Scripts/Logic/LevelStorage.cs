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
        [SerializeField] private ScarecrowEntity _scarecrowEntity;
        [SerializeField] private GameObject _snowContainer;

        private readonly HashSet<IDestructable> _destructables = new();

        public Transform PlayerTransform => _playerTransform;
        public ScarecrowEntity ScarecrowEntity => _scarecrowEntity;

        public void Construct(LifetimeScope lifeTimeScope)
            => AutoInjectAll(lifeTimeScope);
        public void Destruct()
        {
            foreach (IDestructable destructable in _destructables)
                destructable.Destruct();
        }

        private void AutoInjectAll(LifetimeScope lifeTimeScope)
        {
            AllSnowController allSnowController = lifeTimeScope.Container.Resolve(typeof(AllSnowController)) as AllSnowController;
            allSnowController.SnowSquareEntities = _snowContainer.GetComponentsInChildren<SnowSquareEntity>();
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
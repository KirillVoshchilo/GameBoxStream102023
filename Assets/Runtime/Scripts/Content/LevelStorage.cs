using App.Architecture.AppData;
using App.Content.Field;
using App.Logic;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace App.Content
{
    public sealed class LevelStorage : MonoBehaviour
    {
        [SerializeField] private Transform[] _playerSpawnPosition;
        [SerializeField] private GameObject[] _autoInjectObjects;
        [SerializeField] private GameObject _snowContainer;
        [SerializeField] private HelicopterEntity _helicopterEntity;
        [SerializeField] private GameObject _treesContriner;
        [SerializeField] private StorageEntity _storageEntity;

        private readonly HashSet<IDestructable> _destructables = new();
        private ResourceSourceEntity[] _resourceSourceEntities;

        public Transform[] PlayerSpawnPosition => _playerSpawnPosition;
        public HelicopterEntity HelicopterEntity => _helicopterEntity;

        public void Construct(LifetimeScope lifeTimeScope)
            => AutoInjectAll(lifeTimeScope);
        public void ResetAll()
        {
            _storageEntity.ResetInventory();
            foreach (ResourceSourceEntity entity in _resourceSourceEntities)
                entity.Recover();
        }
        public void Destruct()
        {
            foreach (IDestructable destructable in _destructables)
                destructable.Destruct();
        }

        private void AutoInjectAll(LifetimeScope lifeTimeScope)
        {
            AllSnowController allSnowController = lifeTimeScope.Container.Resolve(typeof(AllSnowController)) as AllSnowController;
            allSnowController.SnowSquareEntities = _snowContainer.GetComponentsInChildren<SnowSquareEntity>();
            _resourceSourceEntities = _treesContriner.GetComponentsInChildren<ResourceSourceEntity>();
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
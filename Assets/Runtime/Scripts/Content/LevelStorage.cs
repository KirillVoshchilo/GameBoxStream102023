using App.Architecture.AppData;
using App.Content.Helicopter;
using App.Content.SnowSquare;
using App.Content.Grigory;
using App.Content.Tree;
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
        [SerializeField] private GrigoryEntity _grigoryEntity;

        private readonly HashSet<IDestructable> _destructables = new();
        private TreeEntity[] _resourceSourceEntities;

        public Transform[] PlayerSpawnPosition => _playerSpawnPosition;
        public HelicopterEntity HelicopterEntity => _helicopterEntity;

        public void Construct(LifetimeScope lifeTimeScope)
        {
            AllSnowController allSnowController = lifeTimeScope.Container.Resolve(typeof(AllSnowController)) as AllSnowController;
            allSnowController.SnowSquareEntities = _snowContainer.GetComponentsInChildren<SnowSquareEntity>();
            _resourceSourceEntities = _treesContriner.GetComponentsInChildren<TreeEntity>();
            AutoInjectAll(lifeTimeScope);
        }
        public void ResetLevel()
        {
            _helicopterEntity.IsEnable = true;
            _grigoryEntity.IsEnable = true;
            _grigoryEntity.ResetInventory();
            foreach (TreeEntity entity in _resourceSourceEntities)
                entity.Recover();
        }
        public void Destruct()
        {
            _grigoryEntity.IsEnable = false;
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
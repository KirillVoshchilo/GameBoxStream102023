using App.Architecture.AppData;
using App.Content.Player;
using App.Content.UI;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Architecture.Factories.UI
{
    public sealed class InventoryMenuFactory : ASceneObjectFactory<InventoryPresenter>
    {
        private const string PREFAB = "Prefabs/UI/Inventory";

        private readonly PlayerEntity _playerEntity;
        private readonly VillageTrustSystem _villageTrustSystem;
        private readonly Configuration _configurations;
        private readonly LevelsController _levelsController;
        private readonly LevelTimer _levelTimer;

        [Inject]
        public InventoryMenuFactory(PlayerEntity playerEntity,
            VillageTrustSystem villageTrustSystem,
            Configuration configurations,
            LevelsController levelsController,
            LevelTimer levelTimer)
        {
            _levelTimer = levelTimer;
            _playerEntity = playerEntity;
            _villageTrustSystem = villageTrustSystem;
            _configurations = configurations;
            _levelsController = levelsController;
        }

        public override InventoryPresenter Create()
        {
            InventoryPresenter prefab = Resources.Load<InventoryPresenter>(PREFAB);
            InventoryPresenter instance = Object.Instantiate(prefab);
            instance.Construct(_playerEntity,
                _configurations,
                _levelTimer,
                _villageTrustSystem,
                _levelsController);
            instance.IsEnable = true;
            OnCreated.Invoke(instance);
            return instance;
        }
        public override void Remove(InventoryPresenter obj)
        {
            OnRemoved.Invoke(obj);
            obj.IsEnable = false;
            Object.Destroy(obj.gameObject);
        }
    }
}
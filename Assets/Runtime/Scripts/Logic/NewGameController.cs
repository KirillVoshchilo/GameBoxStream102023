using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Player;
using App.Simples;
using App.Simples.CellsInventory;
using VContainer;

namespace App.Logic
{
    public sealed class NewGameController
    {
        private readonly VillageTrustSystem _villageTrustSystem;
        private readonly AllSnowController _allSnowController;
        private readonly IAppInputSystem _appInputSystem;
        private readonly Configuration _configuration;
        private readonly LevelLoaderSystem _levelLoaderSystem;
        private readonly LevelsController _levelsController;
        private readonly Inventory _playerInventory;

        [Inject]
        public NewGameController(LevelsController levelsController,
            LevelLoaderSystem levelLoaderSystem,
            PlayerEntity playerEntity,
            Configuration configuration,
            IAppInputSystem appInputSystem,
            AllSnowController allSnowController,
            VillageTrustSystem villageTrustSystem)
        {
            _villageTrustSystem = villageTrustSystem;
            _allSnowController = allSnowController;
            _appInputSystem = appInputSystem;
            _configuration = configuration;
            _levelLoaderSystem = levelLoaderSystem;
            _levelsController = levelsController;
            _playerInventory = playerEntity.Get<Inventory>();
        }

        public void StartFirstLevel()
        {
            _villageTrustSystem.ResetTrust();
            _levelLoaderSystem.CurrentLoadedLevel.ResetLevel();
            _levelsController.StartLevel(0);
            SetInitialInventory();
            _levelLoaderSystem.CurrentLoadedLevel.HelicopterEntity.IsEnable = false;
            _levelsController.OnLevelStarted.AddListener(OnFirstLEvelStarted);
            _allSnowController.ResetSnowEntities();
        }

        private void OnFirstLEvelStarted()
        {
            _appInputSystem.EscapeIsEnable = true;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = true;
            _appInputSystem.InteractionIsEnable = false;
            _levelsController.OnLevelStarted.RemoveListener(OnFirstLEvelStarted);
        }
        private void SetInitialInventory()
        {
            _playerInventory.Clear();
            int count = _configuration.StartInventoryConfiguration.Items.Length;
            for (int i = 0; i < count; i++)
            {
                SSOKey key = _configuration.StartInventoryConfiguration.Items[i].Key;
                int quantity = _configuration.StartInventoryConfiguration.Items[i].Count;
                _playerInventory.AddItem(key, quantity);
            }
        }
    }
}
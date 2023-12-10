using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content;
using App.Content.Audio;
using App.Content.Player;
using App.Simples;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace App.Logic
{
    public sealed class LevelsController
    {
        private readonly FallingSnow _fallingSnow;
        private readonly Configuration _configuration;
        private readonly PlayerEntity _playerEntity;
        private readonly VillageTrustSystem _villageTrustSystem;
        private UIController _uiController;
        private readonly IAppInputSystem _appInputSystem;
        private readonly LevelLoaderSystem _levelLoader;
        private readonly Inventory _playerInventory;
        private readonly DefeatController _defeatController;
        private FinishController _finishController;
        private readonly AllSnowController _allSnowController;
        private int _currentLevel;
        private bool _isLevelLoaded;
        private LevelStorage _levelStorage;
        private bool _isGameStarted;
        private SlideShow _currentCutScene;
        private LevelConfiguration _currentLevelConfiguration;
        private readonly LevelTimer _levelTimer;
        private readonly AudioStorage _audioController;
        private readonly BonfireFactory _bonusFactory;
        private readonly SEvent _onLevelStarted = new();
        private readonly WorldCanvasStorage _worldCanvasStorage;

        public FinishController FinishController { get => _finishController; set => _finishController = value; }
        public int CurrentLevel => _currentLevel;
        public UIController UiController { get => _uiController; set => _uiController = value; }
        public SEvent OnLevelStarted => _onLevelStarted;

        [Inject]
        public LevelsController(Configuration configuration,
            PlayerEntity playerEntity,
            VillageTrustSystem villageTrustSystem,
            IAppInputSystem appInputSystem,
            LevelLoaderSystem levelLoader,
            DefeatController defeatController,
            LevelTimer levelTimer,
            AllSnowController allSnowController,
            FallingSnow fallingSnow,
            BonfireFactory bonfireFactory,
            AudioStorage audioController,
            WorldCanvasStorage worldCanvasStorage)
        {
            _audioController = audioController;
            _bonusFactory = bonfireFactory;
            _fallingSnow = fallingSnow;
            _configuration = configuration;
            _playerEntity = playerEntity;
            _villageTrustSystem = villageTrustSystem;
            _appInputSystem = appInputSystem;
            _levelLoader = levelLoader;
            _defeatController = defeatController;
            _playerInventory = playerEntity.Get<Inventory>();
            _levelLoader.LoadScene(LevelLoaderSystem.FIRST_LEVEL, OnCompleteLoading)
                .Forget();
            _levelTimer = levelTimer;
            _allSnowController = allSnowController;
            _worldCanvasStorage = worldCanvasStorage;
        }

        public void StartFirstLevel()
        {
            _isGameStarted = true;
            if (_isLevelLoaded)
            {
                _levelStorage.ResetAll();
                StartLevel(0);
                SetInitialInventory();
            }
        }
        public void ResetLevelController()
        {
            _villageTrustSystem.ResetTrust();
            _fallingSnow.StopSnowing();
            _uiController.CloseCurrentOpenedGamePanel();
            _levelTimer.OnTimeIsOver.RemoveListener(OnTimeHasGone);
            _levelTimer.StopTimer();
            _appInputSystem.EscapeIsEnable = false;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InteractionIsEnable = false;
            HeatData heatData = _playerEntity.Get<HeatData>();
            heatData.CurrentHeat = heatData.DefaultHeatValue;
            heatData.IsFreezing = false;
        }

        public void StartLevel(int levelIndex)
        {
            _currentLevel = levelIndex;
            _currentLevelConfiguration = _configuration.LevelsConfigurations[levelIndex];
            ShowCutScene(_currentLevelConfiguration.CutScene);
        }

        private void SetInitialInventory()
        {
            _playerInventory.Clear();
            int count = _configuration.StartInventoryConfiguration.Items.Length;
            for (int i = 0; i < count; i++)
            {
                Key key = _configuration.StartInventoryConfiguration.Items[i].Key;
                int quantity = _configuration.StartInventoryConfiguration.Items[i].Count;
                _playerInventory.AddItem(key, quantity);
            }
        }
        private void OnCompleteLoading(LevelStorage storage)
        {
            _levelStorage = storage;
            _isLevelLoaded = true;
            if (_isGameStarted)
            {
                StartLevel(0);
                SetInitialInventory();
            }
        }
        private void ShowCutScene(SlideShow slideShow)
        {
            _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.CutSceneMusic);
            _currentCutScene = Object.Instantiate(slideShow);
            _currentCutScene.IsLoop = false;
            _appInputSystem.IsGoNextEnable = true;
            _currentCutScene.OnSlidShowEnded.AddListener(CloseCurrentCutScene);
            _appInputSystem.OnGoNext.AddListener(_currentCutScene.ShowNext);
            _appInputSystem.OnGoNext.AddListener(_audioController.AudioData.SoundTracks.CutSceneChanging.Play);
            _currentCutScene.ShowFirst();
        }
        private void CloseCurrentCutScene()
        {
            _currentCutScene.OnSlidShowEnded.RemoveListener(CloseCurrentCutScene);
            _appInputSystem.OnGoNext.RemoveListener(_currentCutScene.ShowNext);
            _appInputSystem.OnGoNext.RemoveListener(_audioController.AudioData.SoundTracks.CutSceneChanging.Play);
            _appInputSystem.IsGoNextEnable = false;
            Object.Destroy(_currentCutScene.gameObject);
            StartGame();
        }
        private void StartGame()
        {
            _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.LocationSoundtrack);
            if (_currentLevel == 0)
            {
                _levelStorage.HelicopterEntity.IsEnable = false;
                _appInputSystem.EscapeIsEnable = true;
                _appInputSystem.InventoryIsEnable = false;
                _appInputSystem.PlayerMovingIsEnable = true;
                _appInputSystem.InteractionIsEnable = false;
                _allSnowController.ResetSnowEntities();
            }
            else
            {
                ConfigureControl();
                _levelStorage.HelicopterEntity.IsEnable = true;
            }
            _worldCanvasStorage.InteractIcon.gameObject.SetActive(false);
            _bonusFactory.ClearAll();
            _fallingSnow.StartSnowing();
            _playerEntity.transform.position = _levelStorage.PlayerSpawnPosition[_currentLevel].position;
            _defeatController.IsEnable = true;
            _playerEntity.GetComponent<Rigidbody>().useGravity = true;
            ConfigureHeat();
            _uiController.ShowFreezeEffect();
            ConfigureDialoges();
            _uiController.ScareCrowMenuPresenter.CurrentLevel = _currentLevel;
            ConfigureTime();
            _onLevelStarted.Invoke();
        }

        private void ConfigureTime()
        {
            _levelTimer.FullTime = _currentLevelConfiguration.DayTimeRange;
            _levelTimer.StartTimer();
            _levelTimer.OnTimeIsOver.AddListener(OnTimeHasGone);
        }
        private void OnTimeHasGone()
        {
            _fallingSnow.StopSnowing();
            _uiController.CloseCurrentOpenedGamePanel();
            int nextLevel = _currentLevel + 1;
            _levelTimer.OnTimeIsOver.RemoveListener(OnTimeHasGone);
            _appInputSystem.EscapeIsEnable = false;
            _appInputSystem.InventoryIsEnable = false;
            _appInputSystem.PlayerMovingIsEnable = false;
            _appInputSystem.InteractionIsEnable = false;
            HeatData heatData = _playerEntity.Get<HeatData>();
            heatData.CurrentHeat = heatData.DefaultHeatValue;
            heatData.IsFreezing = false;
            if (nextLevel >= _configuration.LevelsConfigurations.Count)
                _finishController.EndTimeFinish();
            else StartLevel(nextLevel);
        }
        private void ConfigureControl()
        {
            _appInputSystem.InteractionIsEnable = true;
            _appInputSystem.EscapeIsEnable = true;
            _appInputSystem.InventoryIsEnable = true;
            _appInputSystem.PlayerMovingIsEnable = true;
        }

        private void ConfigureHeat()
        {
            HeatData heatData = _playerEntity.Get<HeatData>();
            heatData.CurrentHeat = heatData.DefaultHeatValue;
            heatData.IsFreezing = true;
        }
        private void ConfigureDialoges()
        {
            if (_currentLevel == 0)
            {
                _uiController.StorageMenuPresenter.Dialoge = _currentLevelConfiguration.StorageDialogs;
                _uiController.ScareCrowMenuPresenter.Dialoge = _currentLevelConfiguration.ScarecrowDialogs;
                return;
            }
            float currentTrust = _villageTrustSystem.Trust;
            float targetTrust = _configuration.TrustLevels[_currentLevel - 1].Trust;
            if (currentTrust >= targetTrust)
                _uiController.StorageMenuPresenter.Dialoge = _currentLevelConfiguration.StorageDialogeWithTip;
            else _uiController.StorageMenuPresenter.Dialoge = _currentLevelConfiguration.StorageDialogs;
            _uiController.ScareCrowMenuPresenter.Dialoge = _currentLevelConfiguration.ScarecrowDialogs;
        }
    }
}
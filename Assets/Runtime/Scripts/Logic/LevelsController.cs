using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content;
using App.Content.Audio;
using App.Content.Bonfire;
using App.Content.Player;
using App.Simples;
using SimpleComponents.UI;
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
        private readonly EndLevelController _endLevelController;
        private FinishGameController _finishController;
        private int _currentLevel;
        private SCSlideShow _currentCutScene;
        private LevelConfiguration _currentLevelConfiguration;
        private readonly LevelTimer _levelTimer;
        private readonly AudioStorage _audioController;
        private readonly BonfireFactory _bonfireFactory;
        private readonly SEvent _onLevelStarted = new();
        private readonly WorldCanvasStorage _worldCanvasStorage;
        private readonly LevelLoaderSystem _levelLoaderSystem;
        private readonly UIStorage _uIStorage;

        public FinishGameController FinishController { get => _finishController; set => _finishController = value; }
        public int CurrentLevel => _currentLevel;
        public UIController UiController { get => _uiController; set => _uiController = value; }
        public SEvent OnLevelStarted => _onLevelStarted;

        [Inject]
        public LevelsController(Configuration configuration,
            PlayerEntity playerEntity,
            VillageTrustSystem villageTrustSystem,
            IAppInputSystem appInputSystem,
            EndLevelController endLevelController,
            LevelTimer levelTimer,
            FallingSnow fallingSnow,
            BonfireFactory bonfireFactory,
            AudioStorage audioController,
            WorldCanvasStorage worldCanvasStorage,
            LevelLoaderSystem levelLoaderSystem,
            UIStorage uIStorage)
        {
            _levelLoaderSystem = levelLoaderSystem;
            _uIStorage = uIStorage;
            _audioController = audioController;
            _bonfireFactory = bonfireFactory;
            _fallingSnow = fallingSnow;
            _configuration = configuration;
            _playerEntity = playerEntity;
            _villageTrustSystem = villageTrustSystem;
            _appInputSystem = appInputSystem;
            _endLevelController = endLevelController;
            _levelTimer = levelTimer;
            _worldCanvasStorage = worldCanvasStorage;
        }
        public void EndCurrentLevel()
        {
            _levelTimer.StopTimer();
            _fallingSnow.StopSnowing();
            _endLevelController.IsEnable = false;
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

        private void ShowCutScene(SCSlideShow slideShow)
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
            ConfigureControl();
            _levelLoaderSystem.CurrentLoadedLevel.HelicopterEntity.IsEnable = true;
            _worldCanvasStorage.InteractIcon.gameObject.SetActive(false);
            _bonfireFactory.ClearAll();
            _fallingSnow.StartSnowing();
            _playerEntity.transform.position = _levelLoaderSystem.CurrentLoadedLevel.PlayerSpawnPosition[_currentLevel].position;
            _endLevelController.IsEnable = true;
            _playerEntity.GetComponent<Rigidbody>().useGravity = true;
            ConfigureHeat();
            _uiController.ShowFreezeEffect();
            ConfigureDialoges();
            _uIStorage.ScareCrowMenuPresenter.CurrentLevel = _currentLevel;
            ConfigureTime();
            _onLevelStarted.Invoke();
        }
        private void ConfigureTime()
        {
            _levelTimer.FullTime = _currentLevelConfiguration.DayTimeRange;
            _levelTimer.StartTimer();
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
            if (_currentLevel == 0 || _currentLevel == 1)
            {
                _uIStorage.StorageMenuPresenter.Dialoge = _currentLevelConfiguration.StorageDialogs;
                _uIStorage.ScareCrowMenuPresenter.Dialoge = _currentLevelConfiguration.ScarecrowDialogs;
                return;
            }
            float currentTrust = _villageTrustSystem.Trust;
            float targetTrust = _configuration.TrustLevels[_currentLevel - 1];
            if (currentTrust >= targetTrust)
                _uIStorage.StorageMenuPresenter.Dialoge = _currentLevelConfiguration.StorageDialogeWithTip;
            else _uIStorage.StorageMenuPresenter.Dialoge = _currentLevelConfiguration.StorageDialogs;
            _uIStorage.ScareCrowMenuPresenter.Dialoge = _currentLevelConfiguration.ScarecrowDialogs;
        }
    }
}
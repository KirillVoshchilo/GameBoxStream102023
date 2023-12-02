using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Player;
using App.Logic;
using App.Simples;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelsController
{
    private readonly Configuration _configuration;
    private readonly PlayerEntity _playerEntity;
    private readonly VillageTrustSystem _villageTrustSystem;
    private readonly UIController _uiController;
    private readonly IAppInputSystem _appInputSystem;
    private readonly SEvent _onAllLevelsFinished = new();
    private readonly SEvent _onLevelFinished = new();
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

    public SEvent OnAllLevelsFinished => _onAllLevelsFinished;
    public SEvent OnLevelFinished => _onLevelFinished;
    public FinishController FinishController { get => _finishController; set => _finishController = value; }

    public LevelsController(Configuration configuration,
        PlayerEntity playerEntity,
        VillageTrustSystem villageTrustSystem,
        UIController uiController,
        IAppInputSystem appInputSystem,
        LevelLoaderSystem levelLoader,
        DefeatController defeatController,
        LevelTimer levelTimer,
        AllSnowController allSnowController)
    {
        _configuration = configuration;
        _playerEntity = playerEntity;
        _villageTrustSystem = villageTrustSystem;
        _uiController = uiController;
        _appInputSystem = appInputSystem;
        _levelLoader = levelLoader;
        _defeatController = defeatController;
        _playerInventory = playerEntity.Get<Inventory>();
        _levelLoader.LoadScene(LevelLoaderSystem.FIRST_LEVEL, OnCompleteLoading)
            .Forget();
        _levelTimer = levelTimer;
        _allSnowController = allSnowController;
    }

    public void StartFirstLevel()
    {
        _isGameStarted = true;
        if (_isLevelLoaded)
        {
            StartLevel(0);
            SetInitialInventory();
        }
    }

    public void StartLevel(int levelIndex)
    {
        _currentLevel = levelIndex;
        _currentLevelConfiguration = _configuration.LevelsConfigurations[levelIndex];
        ShowCutScene(_currentLevelConfiguration.CutScene);
    }

    private void SetInitialInventory()
    {
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
        _currentCutScene = Object.Instantiate(slideShow);
        _currentCutScene.IsLoop = false;
        _appInputSystem.IsGoNextEnable = true;
        _currentCutScene.OnSlidShowEnded.AddListener(CloseCurrentCutScene);
        _appInputSystem.OnGoNext.AddListener(_currentCutScene.ShowNext);
        _currentCutScene.ShowFirst();
    }
    private void CloseCurrentCutScene()
    {
        _currentCutScene.OnSlidShowEnded.RemoveListener(CloseCurrentCutScene);
        _appInputSystem.OnGoNext.RemoveListener(_currentCutScene.ShowNext);
        _appInputSystem.IsGoNextEnable = false;
        Object.Destroy(_currentCutScene.gameObject);
        StartGame();
    }
    private void StartGame()
    {
        _allSnowController.ResetSnowEntities();
        _playerEntity.transform.position = _levelStorage.PlayerTransform.position;
        _defeatController.IsEnable = true;
        ConfigureControl();
        _playerEntity.GetComponent<Rigidbody>().useGravity = true;
        ConfigureHeat();
        _uiController.ShowFreezeEffect();
        ConfigureDialoges();
        ConfigureScarecrow();
        ConfigureTime();
    }

    private void ConfigureTime()
    {
        _levelTimer.FullTime = _currentLevelConfiguration.DayTimeRange;
        _levelTimer.StartTimer();
        _levelTimer.OnTimeIsOver.AddListener(OnTimeHasGone);
    }
    private void OnTimeHasGone()
    {
        _uiController.CloseCurrentOpenedGamePanel();
        int nextLevel = _currentLevel + 1;
        _levelTimer.OnTimeIsOver.RemoveListener(OnTimeHasGone);
        _appInputSystem.EscapeIsEnable = false;
        _appInputSystem.InventoryIsEnable = false;
        _appInputSystem.PlayerMovingIsEnable = false;
        HeatData heatData = _playerEntity.Get<HeatData>();
        heatData.CurrentHeat = heatData.DefaultHeatValue;
        heatData.IsFreezing = false;
        if (nextLevel >= _configuration.LevelsConfigurations.Count)
            _finishController.EndTimeFinish();
        else StartLevel(nextLevel);
    }
    private void ConfigureControl()
    {
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

    private void ConfigureScarecrow()
    {
        float currentTrust = _villageTrustSystem.Trust;
        int count = _configuration.TrustLevels.Length;
        int ready = 0;
        for (int i = 0; i < count; i++)
        {
            if (currentTrust >= _configuration.TrustLevels[i].Trust)
                ready = i;
        }
        Object.Destroy(_levelStorage.ScarecrowEntity.ScarecrowModel);
        Object.Instantiate(_configuration.TrustLevels[ready].Scarecrow, _levelStorage.ScarecrowEntity.ModelParent.transform);
        _uiController.ScareCrowMenuPresenter.CurrentLevel = _currentLevel;
    }

    private void ConfigureDialoges()
    {
        float currentTrust = _villageTrustSystem.Trust;
        float targetTrust = _configuration.TrustLevels[_currentLevel].Trust;
        if (currentTrust >= targetTrust)
            _uiController.StorageMenuPresenter.Dialoge = _currentLevelConfiguration.StorageDialogeWithTip;
        else _uiController.StorageMenuPresenter.Dialoge = _currentLevelConfiguration.StorageDialogs;
        _uiController.ScareCrowMenuPresenter.Dialoge = _currentLevelConfiguration.ScarecrowDialogs;
    }
}

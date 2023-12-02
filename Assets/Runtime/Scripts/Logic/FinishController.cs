using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Player;
using App.Logic;
using UnityEngine;
using VContainer;

public class FinishController
{
    private readonly FallingSnowController _fallingSnowController;
    private readonly LevelsController _levelsController;
    private readonly Configuration _configuration;
    private readonly IAppInputSystem _appInputSystem;
    private readonly PlayerEntity _playerEntity;
    private readonly UIController _uiController;
    private readonly LevelTimer _levelTimer;
    private readonly AllSnowController _allSnowController;
    private readonly VillageTrustSystem _villageTrustSystem;
    private SlideShow _currentCutScene;

    [Inject]
    public FinishController(LevelsController levelsController,
        Configuration configuration,
        IAppInputSystem appInputSystem,
        PlayerEntity playerEntity,
        AllSnowController allSnowController,
        UIController uiController,
        LevelTimer levelTimer,
        VillageTrustSystem villageTrustSystem,
        FallingSnowController fallingSnowController)
    {
        _fallingSnowController = fallingSnowController;
        _levelsController = levelsController;
        _configuration = configuration;
        _appInputSystem = appInputSystem;
        _playerEntity = playerEntity;
        _allSnowController = allSnowController;
        _uiController = uiController;
        _levelTimer = levelTimer;
        _villageTrustSystem = villageTrustSystem;
    }
    public void Construct()
    {
        _levelsController.FinishController = this;
    }

    public void EscapeFinish()
    {
        StopGame();
        int lastLevel = _configuration.TrustLevels.Length - 1;
        if (_villageTrustSystem.Trust >= _configuration.TrustLevels[lastLevel].Trust)
            ShowCutScene(_configuration.FinalCutScenes.GoodEnd);
        else ShowCutScene(_configuration.FinalCutScenes.EscapeFinal);
    }
    public void EndTimeFinish()
    {
        StopGame();
        int lastLevel = _configuration.TrustLevels.Length - 1;
        if (_villageTrustSystem.Trust >= _configuration.TrustLevels[lastLevel].Trust)
            ShowCutScene(_configuration.FinalCutScenes.AlmostBadEnd);
        else ShowCutScene(_configuration.FinalCutScenes.BadEnd);
    }

    private void ShowCutScene(SlideShow slideShow)
    {
        _currentCutScene = Object.Instantiate(slideShow);
        _currentCutScene.IsLoop = false;
        _appInputSystem.IsGoNextEnable = true;
        _currentCutScene.OnSlidShowEnded.AddListener(OnEndFinalCutScene);
        _appInputSystem.OnGoNext.AddListener(_currentCutScene.ShowNext);
        _currentCutScene.ShowFirst();
    }
    private void OnEndFinalCutScene()
    {
        _currentCutScene.OnSlidShowEnded.RemoveListener(OnEndFinalCutScene);
        _appInputSystem.OnGoNext.RemoveListener(_currentCutScene.ShowNext);
        _appInputSystem.IsGoNextEnable = false;
        Object.Destroy(_currentCutScene.gameObject);
        _uiController.OpenMainMenu();
    }
    private void StopGame()
    {
        _fallingSnowController.StopSnowing();
        _uiController.CloseCurrentOpenedGamePanel();
        _levelTimer.OnTimeIsOver.ClearListeners();
        _levelTimer.PauseTimer();
        _appInputSystem.EscapeIsEnable = false;
        _appInputSystem.InventoryIsEnable = false;
        _appInputSystem.PlayerMovingIsEnable = false;
        HeatData heatData = _playerEntity.Get<HeatData>();
        heatData.CurrentHeat = heatData.DefaultHeatValue;
        heatData.IsFreezing = false;
        _allSnowController.ResetSnowEntities();
    }
}
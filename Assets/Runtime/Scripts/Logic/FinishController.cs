﻿using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Player;
using App.Logic;
using UnityEngine;
using VContainer;

public class FinishController
{
    private readonly AudioController _audioController;
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
        FallingSnowController fallingSnowController,
        AudioController audioController)
    {
        _audioController = audioController;
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
        _levelsController.ResetLevelController();
        int lastLevel = _configuration.TrustLevels.Length - 1;
        if (_villageTrustSystem.Trust >= _configuration.TrustLevels[lastLevel].Trust)
        {
            ShowCutScene(_configuration.FinalCutScenes.GoodEnd);
            _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.FinalMusic_3);
        }
        else
        {
            _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.FinalMusic_4);
            ShowCutScene(_configuration.FinalCutScenes.EscapeFinal);
        }
    }
    public void EndTimeFinish()
    {
        _levelsController.ResetLevelController();
        int lastLevel = _configuration.TrustLevels.Length - 1;
        if (_villageTrustSystem.Trust >= _configuration.TrustLevels[lastLevel].Trust)
        {
            _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.FinalMusic_2);
            SlideShow slideShow = ShowCutScene(_configuration.FinalCutScenes.AlmostBadEnd);
            slideShow.OnSlidShowEnded.AddListener(Application.Quit);
        }
        else
        {
            _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.FinalMusic_1);
            ShowCutScene(_configuration.FinalCutScenes.BadEnd);
        }
    }

    private SlideShow ShowCutScene(SlideShow slideShow)
    {
        _currentCutScene = Object.Instantiate(slideShow);
        _currentCutScene.IsLoop = false;
        _appInputSystem.IsGoNextEnable = true;
        _currentCutScene.OnSlidShowEnded.AddListener(OnEndFinalCutScene);
        _appInputSystem.OnGoNext.AddListener(_currentCutScene.ShowNext);
        _currentCutScene.ShowFirst();
        return _currentCutScene;
    }
    private void OnEndFinalCutScene()
    {
        _currentCutScene.OnSlidShowEnded.RemoveListener(OnEndFinalCutScene);
        _appInputSystem.OnGoNext.RemoveListener(_currentCutScene.ShowNext);
        _appInputSystem.IsGoNextEnable = false;
        Object.Destroy(_currentCutScene.gameObject);
        _uiController.OpenMainMenu();
    }
}
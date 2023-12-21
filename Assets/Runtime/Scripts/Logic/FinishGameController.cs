using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Audio;
using SimpleComponents.UI;
using UnityEngine;
using VContainer;

namespace App.Logic
{
    public sealed class FinishGameController
    {
        private readonly AudioStorage _audioController;
        private readonly LevelsController _levelsController;
        private readonly Configuration _configuration;
        private readonly IAppInputSystem _appInputSystem;
        private readonly UIController _uiController;
        private readonly VillageTrustSystem _villageTrustSystem;
        private SCSlideShow _currentCutScene;

        [Inject]
        public FinishGameController(LevelsController levelsController,
            Configuration configuration,
            IAppInputSystem appInputSystem,
            UIController uiController,
            VillageTrustSystem villageTrustSystem,
            AudioStorage audioController)
        {
            _audioController = audioController;
            _levelsController = levelsController;
            _configuration = configuration;
            _appInputSystem = appInputSystem;
            _uiController = uiController;
            _villageTrustSystem = villageTrustSystem;
        }
        public void Construct()
            => _levelsController.FinishController = this;

        public void EscapeFinish()
        {
            int lastLevel = _configuration.TrustLevels.Length - 1;
            if (_villageTrustSystem.Trust >= _configuration.TrustLevels[lastLevel])
            {
                ShowCutScene(_configuration.FinalCutScenes.GoodEnd);
                _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.FinalMusic_3);
            }
            else
            {
                _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.FinalMusic_4);
                ShowCutScene(_configuration.FinalCutScenes.EscapeFinal);
            }
            _uiController.CloseCurrentOpenedGamePanel();
            _levelsController.EndCurrentLevel();
        }
        public void EndTimeFinish()
        {
            int lastLevel = _configuration.TrustLevels.Length - 1;
            if (_villageTrustSystem.Trust >= _configuration.TrustLevels[lastLevel])
            {
                _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.FinalMusic_2);
                SCSlideShow slideShow = ShowCutScene(_configuration.FinalCutScenes.AlmostBadEnd);
                slideShow.OnSlidShowEnded.AddListener(Application.Quit);
            }
            else
            {
                _audioController.PlayAudioSource(_audioController.AudioData.CycleTracks.FinalMusic_1);
                ShowCutScene(_configuration.FinalCutScenes.BadEnd);
            }
            _uiController.CloseCurrentOpenedGamePanel();
            _levelsController.EndCurrentLevel();
        }

        private SCSlideShow ShowCutScene(SCSlideShow slideShow)
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
}
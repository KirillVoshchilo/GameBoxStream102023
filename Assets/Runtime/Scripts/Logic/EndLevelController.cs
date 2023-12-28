using App.Architecture;
using App.Architecture.AppData;
using App.Content;
using App.Content.Player;
using VContainer;

namespace App.Logic
{
    public sealed class EndLevelController
    {
        private readonly Configuration _configuration;
        private readonly UIController _uiController;
        private readonly LevelsController _levelsController;
        private readonly HeatData _heatData;
        private readonly LevelTimer _levelTimer;
        private readonly FinishGameController _finishController;
        private bool _isEnable;

        public bool IsEnable
        {
            set
            {
                if (_isEnable == value)
                    return;
                _isEnable = value;
                if (value)
                {
                    _levelTimer.OnTimeIsOver.AddListener(OnTimeHasGone);
                    _heatData.OnHeatChanged.AddListener(OnHeatChanged);
                }
                else
                {
                    _levelTimer.OnTimeIsOver.RemoveListener(OnTimeHasGone);
                    _heatData.OnHeatChanged.RemoveListener(OnHeatChanged);
                }
            }
        }

        [Inject]
        public EndLevelController(PlayerEntity playerEntity,
            LevelTimer levelTimer,
            LevelsController levelsController,
            UIController uIController,
            Configuration configuration,
            FinishGameController finishController)
        {
            _configuration = configuration;
            _uiController = uIController;
            _levelsController = levelsController;
            _levelsController.EndLevelController = this;
            _heatData = playerEntity.Get<HeatData>();
            _levelTimer = levelTimer;
            _finishController = finishController;
        }

        private void OnHeatChanged(float obj)
        {
            if (obj <= 0)
                EndLevel();
        }
        private void OnTimeHasGone()
            => EndLevel();
        private void EndLevel()
        {
            _levelTimer.OnTimeIsOver.RemoveListener(OnTimeHasGone);
            int nextLevel = _levelsController.CurrentLevel + 1;
            _uiController.CloseCurrentOpenedGamePanel();
            _levelsController.EndCurrentLevel();
            if (nextLevel >= _configuration.LevelsConfigurations.Count)
                _finishController.EndTimeFinish();
            else _levelsController.StartLevel(nextLevel);
        }
    }
}
using App.Architecture;
using App.Logic;
using TMPro;
using UnityEngine;

namespace App.Content.UI
{
    public sealed class GameWatchPresenter : MonoBehaviour
    {
        private const string TIME_FORMAT = "00";

        [SerializeField] private TextMeshProUGUI _timer;

        private LevelsController _levelController;
        private LevelTimer _levelTimer;

        public void Construct(LevelTimer levelTimer,
            LevelsController levelsController)
        {
            _levelTimer = levelTimer;
            _levelController = levelsController;
            levelTimer.OnTimeHasChanged.AddListener(ShowTime);
        }
        public void Destruct()
        {
            _levelTimer.OnTimeHasChanged.RemoveListener(ShowTime);
        }

        private void ShowTime(float obj)
        {
            int value = (int)obj;
            int seconds = value % 60;
            int minutes = (value - seconds) / 60;
            int day = _levelController.CurrentLevel + 1;
            _timer.text = $"{day} - {minutes.ToString(TIME_FORMAT)}:{seconds.ToString(TIME_FORMAT)}";
        }
    }
}
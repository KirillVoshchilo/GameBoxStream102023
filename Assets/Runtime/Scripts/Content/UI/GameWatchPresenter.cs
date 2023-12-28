using App.Architecture;
using App.Logic;
using TMPro;
using UnityEngine;
using VContainer;

namespace App.Content.UI
{
    public sealed class GameWatchPresenter : MonoBehaviour
    {
        private const string TIME_FORMAT = "00";

        [SerializeField] private TextMeshProUGUI _timer;

        private LevelsController _levelController;

        [Inject]
        public void Construct(LevelTimer levelTimer,
            LevelsController levelsController)
        {
            _levelController = levelsController;
            levelTimer.OnTimeHasChanged.AddListener(ShowTime);
        }

        private void ShowTime(float obj)
        {
            int value = (int)obj;
            int seconds = value % 60;
            int minutes = (value - seconds) / 60;
            _timer.text = $"{_levelController.CurrentLevel} - {minutes.ToString(TIME_FORMAT)}:{seconds.ToString(TIME_FORMAT)}";
        }
    }
}
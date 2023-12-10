using TMPro;
using UnityEngine;
using VContainer;

public sealed class GameWatchPresenter : MonoBehaviour
{
    private const string TIME_FORMAT = "00";

    [SerializeField] private TextMeshProUGUI _timer;

    [Inject]
    public void Construct(LevelTimer levelTimer)
    {
        levelTimer.OnTimeHasChanged.AddListener(ShowTime);
    }

    private void ShowTime(float obj)
    {
        int value = (int)obj;
        int seconds = value % 60;
        int minutes = (value - seconds) / 60;
        _timer.text = $"{minutes.ToString(TIME_FORMAT)}:{seconds.ToString(TIME_FORMAT)}";
    }
}

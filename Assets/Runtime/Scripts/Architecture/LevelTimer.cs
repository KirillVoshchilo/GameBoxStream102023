using App.Simples;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelTimer
{
    private readonly SEvent _onTimeIsOver = new();
    private float _fullTime;
    private float _currentTime;
    private bool _isEnable;

    public float FullTime { get => _fullTime; set => _fullTime = value; }
    public SEvent OnTimeIsOver => _onTimeIsOver;

    public void StartTimer()
    {
        _isEnable = true;
        _currentTime = _fullTime;
        TimerProcess()
            .Forget();
    }

    public void ContinueTimer()
    {
        _isEnable = true;
        TimerProcess()
            .Forget();
    }
    public void PauseTimer()
    {
        _isEnable = false;
    }
    public void StopTimer()
    {
        _isEnable = false;
        _currentTime = 0;
    }

    private async UniTask TimerProcess()
    {
        while (_isEnable && _currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            await UniTask.NextFrame();
        }
        if (_currentTime < 0)
            _currentTime = 0;
        _onTimeIsOver.Invoke();
    }
}

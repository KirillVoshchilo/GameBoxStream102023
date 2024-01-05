using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Simples
{
    public sealed class SBackwardTimer
    {
        private readonly SEvent _onTimeIsOver = new();
        private readonly SEvent<float> _onTimeHasChanged = new();
        private float _fullTime;
        private float _currentTime;
        private bool _isEnable;
        private bool _isPaused;

        public float FullTime { get => _fullTime; set => _fullTime = value; }
        public SEvent OnTimeIsOver => _onTimeIsOver;
        public SEvent<float> OnTimeHasChanged => _onTimeHasChanged;
        public float CurrentTime => _currentTime;

        public void StartTimer()
        {
            _isEnable = true;
            _currentTime = _fullTime;
            _onTimeHasChanged.Invoke(_currentTime);
            TimerProcess()
                .Forget();
        }
        public void ContinueTimer()
        {
            _isPaused = false;
            TimerProcess()
                .Forget();
        }
        public void PauseTimer()
            => _isPaused = true;
        public void StopTimer()
        {
            _isEnable = false;
            _currentTime = 0;
        }

        private async UniTask TimerProcess()
        {
            while (_isEnable && _currentTime > 0 && !_isPaused)
            {
                _currentTime -= Time.deltaTime;
                if (_currentTime < 0)
                    _currentTime = 0;
                _onTimeHasChanged.Invoke(_currentTime);
                await UniTask.NextFrame();
            }
            if (!_isPaused)
                _onTimeIsOver.Invoke();
        }
    }
}
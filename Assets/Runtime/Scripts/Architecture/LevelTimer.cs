using App.Simples;

namespace App.Architecture
{
    public sealed class LevelTimer
    {
        private readonly SBackwardTimer _timer = new();

        public float FullTime { get => _timer.FullTime; set => _timer.FullTime = value; }
        public SEvent OnTimeIsOver => _timer.OnTimeIsOver;
        public SEvent<float> OnTimeHasChanged => _timer.OnTimeHasChanged;
        public float CurrenTime => _timer.CurrentTime;

        public void StartTimer()
            => _timer.StartTimer();
        public void ContinueTimer()
            => _timer.ContinueTimer();
        public void PauseTimer()
            => _timer.PauseTimer();
        public void StopTimer()
            => _timer.StopTimer();
    }
}
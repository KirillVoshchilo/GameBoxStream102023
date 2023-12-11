using App.Architecture.AppData;
using App.Simples;
using VContainer;

namespace App.Architecture
{
    public sealed class VillageTrustSystem
    {
        private readonly SEvent<float> _onTrustChanged = new();
        private readonly SEvent<int> _onTrustLevelChanged = new();
        private readonly Configuration _configuration;
        private float _currentTrust = 0;
        private int _currentTrustLevel = 0;

        public float Trust => _currentTrust;
        public SEvent<float> OnTrustChanged => _onTrustChanged;
        public int CurrentTrustLevel => _currentTrustLevel;
        public SEvent<int> OnTrustLevelChanged => _onTrustLevelChanged;

        [Inject]
        public VillageTrustSystem(Configuration configuration)
            => _configuration = configuration;

        public void ResetTrust()
        {
            _currentTrust = 0;
            _onTrustChanged.Invoke(_currentTrust);
            int count = _configuration.TrustLevels.Length;
            int i;
            for (i = 0; i < count; i++)
            {
                if (_currentTrust < _configuration.TrustLevels[i])
                    break;
            }
            if (_currentTrustLevel < i)
            {
                _currentTrustLevel = i;
                _onTrustLevelChanged.Invoke(_currentTrustLevel);
            }
        }
        public void AddTrust(float value)
        {
            _currentTrust += value;
            _onTrustChanged.Invoke(_currentTrust);
            int count = _configuration.TrustLevels.Length;
            int i;
            for (i = 0; i < count; i++)
            {
                if (_currentTrust < _configuration.TrustLevels[i])
                    break;
            }
            if (_currentTrustLevel < i)
            {
                _currentTrustLevel = i;
                _onTrustLevelChanged.Invoke(_currentTrustLevel);
            }
        }
    }
}
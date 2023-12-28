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
            SetTrust(0);
            DefineTrustLevel(_currentTrust);
        }
        public void AddTrust(float value)
        {
            SetTrust(_currentTrust + value);
            DefineTrustLevel(_currentTrust);
        }

        private void SetTrust(float trust)
        {
            _currentTrust = trust;
            _onTrustChanged.Invoke(trust);
        }
        private void DefineTrustLevel(float trust)
        {
            int trustLevel = 0;
            int i;
            int count = _configuration.TrustLevels.Length;
            for (i = 0; i < count; i++)
            {
                if (trust < _configuration.TrustLevels[i])
                    break;
            }
            if (trustLevel < i)
                trustLevel = i;
            if (trustLevel != _currentTrustLevel)
            {
                _currentTrustLevel = trustLevel;
                _onTrustLevelChanged.Invoke(_currentTrustLevel);
            }
        }
    }
}
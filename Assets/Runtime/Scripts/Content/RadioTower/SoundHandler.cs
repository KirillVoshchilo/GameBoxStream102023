using UnityEngine;

namespace App.Content.RadioTower
{
    public class SoundHandler
    {
        private readonly RadioTowerData _data;

        private bool _isEnable;
        private bool _firstActivation = true;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (value == _isEnable)
                    return;

                _isEnable = value;

                if (value)
                {
                    _data.FinishGameController.OnGameFinished.AddListener(OnGameFinished);
                    _data.UiController.OnUIOpened.AddListener(OnUIOpened);
                }
                else
                {
                    _data.FinishGameController.OnGameFinished.RemoveListener(OnGameFinished);
                    _data.UiController.OnUIOpened.RemoveListener(OnUIOpened);
                }
            }
        }

        public SoundHandler(RadioTowerData data)
        {
            _data = data;
            _data.Activation = new Activation(Activate, Deactivate);
        }

        private void OnUIOpened(bool obj)
        {
            if (!_data.Activation.IsActive)
                return;

            if (obj)
            {
                _data.AudioSource.Pause();
            }
            else
            {
                _data.AudioSource.UnPause();
            }
        }
        private void OnGameFinished(bool obj)
        {
            if (!_data.Activation.IsActive)
                return;

            if (obj)
            {
                _data.AudioSource.Stop();
            }
        }

        private void Deactivate()
        {
            _data.AudioSource.Pause();
        }
        private void Activate()
        {
            if (_firstActivation)
            {
                _data.AudioSource.Play();
                _firstActivation = false;
            }
            else
            {
                _data.AudioSource.UnPause();
            }
        }
    }
}
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Content.Player
{
    public sealed class HeatHandler
    {
        private readonly PlayerData _playerData;

        private bool _isEnable;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (value == _isEnable)
                    return;
                _isEnable = value;
                HeatData heatData = _playerData.HeatData;
                if (value)
                {
                    heatData.OnFreazingStateChanged.AddListener(OnFreezingStateChanged);
                    heatData.OnHeatChanged.AddListener(OnHeatChanged);
                }
                else
                {
                    heatData.OnFreazingStateChanged.RemoveListener(OnFreezingStateChanged);
                    heatData.OnHeatChanged.RemoveListener(OnHeatChanged);
                }
            }
        }

        public HeatHandler(PlayerData playerData)
        {
            _playerData = playerData;
            HeatData heatData = playerData.HeatData;
            heatData.CurrentHeat = heatData.DefaultHeatValue;
            heatData.CurrentFreezingRate = heatData.DefaultFreezingRate;
        }

        private void OnFreezingStateChanged(bool obj)
        {
            if (!obj)
                return;
            FreezingProcess()
                .Forget();
        }
        private void OnHeatChanged(float obj)
        {
            if (!_playerData.HasCoughed && obj < _playerData.HeatData.DefaultHeatValue * 0.2)
            {
                _playerData.HasCoughed = true;
                _playerData.CoughSound.Play();
            }
            if (_playerData.HasCoughed && obj > _playerData.HeatData.DefaultHeatValue * 0.2)
                _playerData.HasCoughed = false;
        }
        private async UniTask FreezingProcess()
        {
            HeatData heatData = _playerData.HeatData;
            while (heatData.IsFreezing && _isEnable)
            {
                float value = heatData.CurrentFreezingRate * Time.deltaTime;
                heatData.CurrentHeat = Mathf.Clamp(heatData.CurrentHeat - value, 0, heatData.DefaultHeatValue);
                await UniTask.NextFrame();
            }
        }
    }
}
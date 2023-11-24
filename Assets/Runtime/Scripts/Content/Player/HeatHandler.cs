using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Content.Player
{
    public sealed class HeatHandler
    {
        private readonly HeatData _heatData;

        public HeatHandler(PlayerData playerData)
        {
            _heatData = playerData.HeatData;
            playerData.HeatData.CurrentHeat = playerData.HeatData.DefaultHeatValue;
            playerData.HeatData.CurrentFreezingRate = playerData.HeatData.DefaultFreezingRate;
            playerData.HeatData.OnFreazingStateChanged.AddListener(OnFreezingStateChanged);
        }

        private void OnFreezingStateChanged(bool obj)
        {
            if (!obj)
                return;
            FreezingProcess()
                .Forget();
        }

        private async UniTask FreezingProcess()
        {
            while (_heatData.IsFreezing)
            {
                float value = _heatData.CurrentFreezingRate * Time.deltaTime;
                _heatData.CurrentHeat = Mathf.Clamp(_heatData.CurrentHeat - value, 0, _heatData.DefaultHeatValue);
                await UniTask.NextFrame();
            }
        }
    }
}
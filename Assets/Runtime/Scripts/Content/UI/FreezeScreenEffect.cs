using App.Architecture.AppData;
using App.Content.Player;
using UnityEngine;
using UnityEngine.UI;

namespace App.Content.UI
{
    public sealed class FreezeScreenEffect : MonoBehaviour
    {
        [SerializeField] private float _minSide;
        [SerializeField] private float _maxSide;
        [SerializeField] private Image _freezeImage;

        private HeatData _heatData;
        private Material _freezeMaterial;

        public void Construct(PlayerEntity playerEntity)
        {
            _heatData = playerEntity.Get<HeatData>();
            _freezeMaterial = _freezeImage.materialForRendering;
            _heatData.OnHeatNormalizedChanged.AddListener(OnHeatChanged);
        }
        public void Destruct()
        {
            _heatData.OnHeatNormalizedChanged.RemoveListener(OnHeatChanged);
        }

        private void OnHeatChanged(float value)
        {
            float range = _maxSide - _minSide;
            float resultSide = _minSide + (range * value);
            _freezeMaterial.SetFloat(ShaderKeys.SideValue, resultSide);
        }
    }
}
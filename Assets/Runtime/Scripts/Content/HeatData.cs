using App.Simples;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Content
{
    [Serializable]
    public sealed class HeatData
    {
        [SerializeField] private float _defaultHeatValue;
        [SerializeField] private float _defaultFreezingRate;

        private readonly SEvent<float> _onHeatChanged = new();
        private readonly SEvent<float> _onHeatNormalizedChanged = new();
        private readonly SEvent<bool> _onFreazingStateChanged = new();
        private readonly Dictionary<string, float> _freezingMultipliers = new();
        private readonly List<HeatCetner> _heatSources = new();
        private float _currentFreezingRate;
        private float _currentHeat;
        private bool _isFreezing;

        public float DefaultHeatValue => _defaultHeatValue;
        public float DefaultFreezingRate => _defaultFreezingRate;
        public float CurrentHeat
        {
            get => _currentHeat;
            set
            {
                _currentHeat = value;
                _onHeatChanged.Invoke(value);
                _onHeatNormalizedChanged.Invoke(_currentHeat / _defaultHeatValue);
            }
        }
        public Dictionary<string, float> FreezingMultipliers => _freezingMultipliers;
        public List<HeatCetner> HeatSources => _heatSources;
        public float CurrentFreezingRate { get => _currentFreezingRate; set => _currentFreezingRate = value; }
        public bool IsFreezing
        {
            get => _isFreezing;
            set
            {
                _isFreezing = value;
                _onFreazingStateChanged.Invoke(value);
            }
        }
        public SEvent<float> OnHeatChanged => _onHeatChanged;
        public SEvent<bool> OnFreazingStateChanged => _onFreazingStateChanged;
        public SEvent<float> OnHeatNormalizedChanged => _onHeatNormalizedChanged;
    }
}
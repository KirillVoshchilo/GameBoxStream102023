using App.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Content
{
    [Serializable]
    public sealed class HeatCenter
    {
        [SerializeField] private SCTriggerComponent _trigger;

        private readonly HashSet<HeatData> _heatDatas = new();

        public Transform HeatZone
        {
            get
            {
                if (_trigger == null)
                    return null;
                return _trigger.transform;
            }
        }

        public void Construct()
        {
            _trigger.OnEnter.AddListener(OnEnterHeatZone);
            _trigger.OnExit.AddListener(OnExitHeatZone);
        }
        public void Destruct()
        {
            _trigger.OnEnter.AddListener(OnEnterHeatZone);
            _trigger.OnExit.AddListener(OnExitHeatZone);
            foreach (HeatData heatData in _heatDatas)
            {
                heatData.HeatSources.Remove(this);
                if (heatData.HeatSources.Count == 0)
                    heatData.IsFreezing = true;
            }
            _heatDatas.Clear();
        }

        private void OnExitHeatZone(Collider collider)
        {
            if (!collider.TryGetComponent(out IEntity entity))
                return;
            HeatData heatData = entity.Get<HeatData>();
            if (heatData != null)
            {
                heatData.HeatSources.Remove(this);
                if (heatData.HeatSources.Count == 0)
                    heatData.IsFreezing = true;
                _heatDatas.Remove(heatData);
            }
        }
        private void OnEnterHeatZone(Collider collider)
        {
            if (!collider.TryGetComponent(out IEntity entity))
                return;
            HeatData heatData = entity.Get<HeatData>();
            if (heatData != null)
            {
                heatData.HeatSources.Add(this);
                heatData.IsFreezing = false;
                heatData.CurrentHeat = heatData.DefaultHeatValue;
                _heatDatas.Add(heatData);
            }
        }
    }
}
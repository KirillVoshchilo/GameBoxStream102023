using App.Components;
using App.Content.Entities;
using App.Content.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeatCetner
{
    [SerializeField] private TriggerComponent _playerSearcher;

    private readonly HashSet<HeatData> _heatDatas = new();

    public Transform HeatZone
    {
        get
        {
            if (_playerSearcher == null)
                return null;
            return _playerSearcher.transform;
        }
    }

    public void Construct()
    {
        _playerSearcher.OnEnter.AddListener(OnEnterHeatZone);
        _playerSearcher.OnExit.AddListener(OnExitHeatZone);
    }
    public void Destruct()
    {
        _playerSearcher.OnEnter.AddListener(OnEnterHeatZone);
        _playerSearcher.OnExit.AddListener(OnExitHeatZone);
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
using App.Content.Player;
using App.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class DefeatController
{
    private UIController _uiController;
    private PlayerEntity _playerEntity;
    private HeatData _heatData;
    private bool _isEnable;

    public bool IsEnable
    {
        set
        {
            _isEnable = value;
            if (value)
                _heatData.OnHeatChanged.AddListener(OnHeatChanged);
            else _heatData.OnHeatChanged.RemoveListener(OnHeatChanged);
        }
    }

    [Inject]
    public DefeatController(UIController uiController,
        PlayerEntity playerEntity)
    {
        _uiController = uiController;
        _playerEntity = playerEntity;
        _heatData = playerEntity.Get<HeatData>();
    }

    private void OnHeatChanged(float obj)
    {
        if (obj <= 0)
        {
            _uiController.OpenDefeatCanvas();
        }
    }
}

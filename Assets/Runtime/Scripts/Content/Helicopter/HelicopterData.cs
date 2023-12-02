using App.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HelicopterData 
{
    [SerializeField] private TriggerComponent _triggerComponent;

    private FinishController _finishController; 

    public TriggerComponent TriggerComponent => _triggerComponent;
    public FinishController FinishController { get => _finishController; set => _finishController = value; }
}

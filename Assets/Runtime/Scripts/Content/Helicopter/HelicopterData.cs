using App.Components;
using App.Logic;
using System;
using UnityEngine;

namespace App.Content.Helicopter
{
    [Serializable]
    public class HelicopterData
    {
        [SerializeField] private SCTriggerComponent _triggerComponent;

        private FinishController _finishController;

        public SCTriggerComponent TriggerComponent => _triggerComponent;
        public FinishController FinishController { get => _finishController; set => _finishController = value; }
    }
}
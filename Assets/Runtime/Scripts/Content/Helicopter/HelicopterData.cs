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

        private FinishGameController _finishController;

        public SCTriggerComponent TriggerComponent => _triggerComponent;
        public FinishGameController FinishController { get => _finishController; set => _finishController = value; }
    }
}
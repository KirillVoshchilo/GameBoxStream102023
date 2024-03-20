using System.Collections.Generic;
using UnityEngine;

namespace App.Content.Player
{
    public sealed class EnvironmentObjectsSwitcher
    {
        private readonly PlayerData _data;
        private bool _isEnable;
        private List<Activation> _activatedObjects = new();

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (value == _isEnable)
                    return;
                _isEnable = value;
                if (value)
                {
                    _data.EnablerSensor.OnEnter.AddListener(TryToActivate);
                    _data.EnablerSensor.OnExit.AddListener(TryToDiactivate);
                }
                else
                {
                    _data.EnablerSensor.OnEnter.RemoveListener(TryToActivate);
                    _data.EnablerSensor.OnExit.RemoveListener(TryToDiactivate);
                }
            }
        }



        public EnvironmentObjectsSwitcher(PlayerData playerData)
        {
            _data = playerData;
        }
        private void TryToDiactivate(Collider collider)
        {
            if (!collider.TryGetComponent(out IEntity entity))
                return;

            Activation activation = entity.Get<Activation>();

            if (activation == null)
                return;
            if (!_activatedObjects.Contains(activation))
                return;

            activation.IsActive = false;
            _activatedObjects.Remove(activation);
        }
        private void TryToActivate(Collider collider)
        {
            if (!collider.TryGetComponent(out IEntity entity))
                return;

            Activation activation = entity.Get<Activation>();

            if (activation == null)
                return;
            if (_activatedObjects.Contains(activation))
                return;

            activation.IsActive = true;
            _activatedObjects.Add(activation);
        }
    }
}
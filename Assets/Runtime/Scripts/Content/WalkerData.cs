using App.Simples;
using System.Collections.Generic;
using UnityEngine;

namespace App.Content
{
    public sealed class WalkerData
    {
        private readonly Rigidbody _rigidbody;
        private readonly Dictionary<string, float> _speedMultipliers = new();
        private readonly SEvent _onMovingStarted = new();
        private readonly SEvent _onMovingStopped = new();
        private readonly SEvent<float> _onSpeedChanged = new();
        private readonly SEvent<bool> _onMovingEnableChanged = new();
        private bool _isMoving;
        private float _movingSpeed;
        private Vector3 _movingDIrection;
        private bool _isMovingEnable;

        public Dictionary<string, float> SpeedMultipliers => _speedMultipliers;
        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                _isMoving = value;
                if (value)
                    _onMovingStarted.Invoke();
                else _onMovingStopped.Invoke();
            }
        }
        public float MovingSpeed
        {
            get => _movingSpeed;
            set
            {
                if (_movingSpeed == value)
                    return;
                _movingSpeed = value;
                _onSpeedChanged.Invoke(value);
            }
        }
        public Vector3 Position => _rigidbody.position;
        public Vector3 EulerRotation => _rigidbody.rotation.eulerAngles;
        public Vector3 MovingDirection { get => _movingDIrection; set => _movingDIrection = value; }
        public SEvent OnMovingStarted => _onMovingStarted;
        public SEvent OnMovingStopped => _onMovingStopped;
        public SEvent<float> OnSpeedChanged => _onSpeedChanged;
        public bool IsMovingEnable
        {
            get => _isMovingEnable;
            set
            {
                if (value != _isMovingEnable)
                    _onMovingEnableChanged.Invoke(value);
                _isMovingEnable = value;
            }
        }
        public SEvent<bool> OnMovingEnableChanged => _onMovingEnableChanged;

        public WalkerData(Rigidbody rigidbody)
            => _rigidbody = rigidbody;
    }
}
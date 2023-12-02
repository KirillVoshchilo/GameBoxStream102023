using App.Simples;
using System.Collections.Generic;
using UnityEngine;

namespace App.Content.Player
{
    public class WalkerData
    {
        private readonly Rigidbody _rigidbody;
        private readonly Dictionary<string, float> _speedMultipliers = new();
        private readonly SEvent _onMovingStarted = new();
        private readonly SEvent _onMovingStopped = new();
        private bool _isMoving;
        private float _movingSpeed;
        private Vector3 _movingDIrection;

        public WalkerData(Rigidbody rigidbody)
            => _rigidbody = rigidbody;

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
        public float MovingSpeed { get => _movingSpeed; set => _movingSpeed = value; }
        public Rigidbody Rigidbody => _rigidbody;
        public Transform Transform => _rigidbody.transform;
        public Vector3 Position => _rigidbody.position;
        public Vector3 EulerRotation => _rigidbody.rotation.eulerAngles;
        public Vector3 MovingDirection { get => _movingDIrection; set => _movingDIrection = value; }
        public SEvent OnMovingStarted => _onMovingStarted;
        public SEvent OnMovingStopped => _onMovingStopped;
    }
}
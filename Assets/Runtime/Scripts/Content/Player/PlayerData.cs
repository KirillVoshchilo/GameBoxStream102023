using App.Architecture;
using App.Architecture.AppInput;
using App.Components;
using App.Content.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Content.Player
{
    [Serializable]
    public sealed class PlayerData
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _transform;
        [SerializeField] private float _defaultMovingSpeed;
        [SerializeField] private TriggerComponent _triggerComponent;
        [SerializeField] private float _defaultHeatValue;

        private readonly Dictionary<string, float> _heatMultipliers = new();
        private float _currentHeat;
        private PlayerInventory _playerInventory;
        private Transform _mainCameraTransform;
        private InteractionComp _interactionEntity;
        private IAppInputSystem _appInputSystem;
        private bool _isEnable;
        private WalkerData _walker;

        public Transform MainCameraTransform { get => _mainCameraTransform; set => _mainCameraTransform = value; }
        public InteractionComp InteractionEntity { get => _interactionEntity; set => _interactionEntity = value; }
        public float DefaultMovingSpeed => _defaultMovingSpeed;
        public Rigidbody Rigidbody => _rigidbody;
        public Transform Transform => _transform;
        public TriggerComponent TriggerComponent => _triggerComponent;
        public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
        public PlayerInventory PlayerInventory { get => _playerInventory; set => _playerInventory = value; }
        public bool IsEnable { get => _isEnable; set => _isEnable = value; }
        public float DefaultHeatValue => _defaultHeatValue;
        public float CurrentHeat { get => _currentHeat; set => _currentHeat = value; }
        public Dictionary<string, float> HeatMultipliers => _heatMultipliers;
        public WalkerData Walker { get => _walker; set => _walker = value; }
    }
}
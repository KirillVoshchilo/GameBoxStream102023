using App.Architecture.AppInput;
using App.Components;
using App.Content.Entities;
using App.Logic;
using System;
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
        [SerializeField] private HeatData _heatData;
        [SerializeField] private Transform _bonfireTargetPosition;
        [SerializeField] private float _buildCheckcolliderSize;
        [SerializeField] private InteractionRequirementsComp _bonfireBuildRequirements;
        [SerializeField] private Animator _animator;

        private BonfireFactory _bonfireFactory;
        private Inventory _playerInventory;
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
        public Inventory PlayerInventory { get => _playerInventory; set => _playerInventory = value; }
        public bool IsEnable { get => _isEnable; set => _isEnable = value; }
        public WalkerData Walker { get => _walker; set => _walker = value; }
        public Transform BonfireTargetPosition => _bonfireTargetPosition;
        public HeatData HeatData { get => _heatData; set => _heatData = value; }
        public InteractionRequirementsComp BonfireBuildRequirements => _bonfireBuildRequirements;
        public float BuildCheckcolliderSize => _buildCheckcolliderSize;
        public BonfireFactory BonfireFactory { get => _bonfireFactory; set => _bonfireFactory = value; }
        public Animator Animator  => _animator;
    }
}
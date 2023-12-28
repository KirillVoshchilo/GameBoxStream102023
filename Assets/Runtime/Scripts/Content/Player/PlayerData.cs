using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Components;
using App.Content.Bonfire;
using App.Content.SnowSquare;
using App.Simples;
using App.Simples.CellsInventory;
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
        [SerializeField] private SCTriggerComponent _triggerComponent;
        [SerializeField] private HeatData _heatData;
        [SerializeField] private Transform _bonfireTargetPosition;
        [SerializeField] private float _buildCheckcolliderSize;
        [SerializeField] private InteractionRequirementsComp _bonfireBuildRequirements;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _axeParent;
        [SerializeField] private SSOKey _axeCategory;
        [SerializeField] private PlayerAnimationsEvents _playerAnimationsEvents;
        [SerializeField] private AudioSource _axeHitSound;
        [SerializeField] private AudioSource _stepSound;
        [SerializeField] private AudioSource _coughSound;

        private Configuration _configuration;
        private BonfireFactory _bonfireFactory;
        private Inventory _playerInventory;
        private Transform _mainCameraTransform;
        private InteractionComp _interactionEntity;
        private IAppInputSystem _appInputSystem;
        private WalkerData _walker;
        private GameObject _currentAxeModel;
        private bool _hasCoughed;

        public Transform MainCameraTransform { get => _mainCameraTransform; set => _mainCameraTransform = value; }
        public InteractionComp InteractionEntity { get => _interactionEntity; set => _interactionEntity = value; }
        public float DefaultMovingSpeed => _defaultMovingSpeed;
        public Rigidbody Rigidbody => _rigidbody;
        public Transform Transform => _transform;
        public SCTriggerComponent TriggerComponent => _triggerComponent;
        public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
        public Inventory PlayerInventory { get => _playerInventory; set => _playerInventory = value; }
        public WalkerData Walker { get => _walker; set => _walker = value; }
        public Transform BonfireTargetPosition => _bonfireTargetPosition;
        public HeatData HeatData { get => _heatData; set => _heatData = value; }
        public InteractionRequirementsComp BonfireBuildRequirements => _bonfireBuildRequirements;
        public float BuildCheckcolliderSize => _buildCheckcolliderSize;
        public BonfireFactory BonfireFactory { get => _bonfireFactory; set => _bonfireFactory = value; }
        public Animator Animator => _animator;
        public Transform AxeParent => _axeParent;
        public GameObject CurrentAxeModel { get => _currentAxeModel; set => _currentAxeModel = value; }
        public Configuration Configuration { get => _configuration; set => _configuration = value; }
        public SSOKey AxeCategory => _axeCategory;
        public PlayerAnimationsEvents PlayerAnimationsEvents => _playerAnimationsEvents;
        public bool CanBuildBonfire
        {
            get
            {
                if (!CheckRequirements())
                    return false;
                if (!CheckSpace())
                    return false;
                return true;
            }
        }
        public AudioSource AxeHitSound  => _axeHitSound; 
        public AudioSource StepSound  => _stepSound;
        public AudioSource CoughSound => _coughSound;
        public bool HasCoughed { get => _hasCoughed; set => _hasCoughed = value; }

        private bool CheckAlternative(Alternatives alternatives)
        {
            foreach (ItemCount itemCount in alternatives.Requirements)
            {
                if (_playerInventory.GetCount(itemCount.Key) < itemCount.Count)
                    return false;
            }
            return true;
        }
        private bool CheckRequirements()
        {
            foreach (Alternatives alt in _bonfireBuildRequirements.Alternatives)
            {
                if (CheckAlternative(alt))
                    return true;
            }
            return false;
        }
        private bool CheckSpace()
        {
            Vector3 position = _bonfireTargetPosition.position;
            RaycastHit[] hitsInfo = Physics.SphereCastAll(position, _buildCheckcolliderSize, Vector3.down, 1);
            foreach (RaycastHit hitInfo in hitsInfo)
            {
                if (!hitInfo.collider.gameObject.TryGetComponent(out SnowSquareEntity snowSquareEntity))
                    return false;
            }
            return true;
        }
    }
}
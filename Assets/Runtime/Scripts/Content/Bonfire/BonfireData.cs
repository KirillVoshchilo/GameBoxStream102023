using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Architecture.Factories.UI;
using App.Content.UI;
using App.Simples.CellsInventory;
using System;
using UnityEngine;

namespace App.Content.Bonfire
{
    [Serializable]
    public sealed class BonfireData
    {
        [SerializeField] private float _defaultLifetime;
        [SerializeField] private GameObject _rootObject;
        [SerializeField] private InteractionRequirementsComp _interactionRequirements;
        [SerializeField] private Transform _interactionIconTransform;
        [SerializeField] private float _interactTime;
        [SerializeField] private float _maxLightScale;
        [SerializeField] private float _minLightScale;
        [SerializeField] private HeatCenter _heatCenter;
        [SerializeField] private AudioSource _kindlingBonfire;
        [SerializeField] private AudioSource _burningFire;
        [SerializeField] private AudioSource _fireRefresh;

        private readonly EntityFlags _flags = new(new string[] { Flags.BONFIRE });
        private BonfireFactory _bonfireFactory;
        private InteractionIconFactory _interactionIconFactory;
        private float _currentLifetime;
        private readonly InteractionComp _interactableComp = new();
        private Inventory _playerInventory;
        private IAppInputSystem _appInputSystem;
        private bool _isInteracting = false;
        private bool _isInteractable;
        private InteractIcon _interactionIcon;

        public Alternatives[] Alternatives => _interactionRequirements.Alternatives;
        public float InteractTime => _interactTime;
        public InteractionComp InteractableComp => _interactableComp;
        public Vector3 InteractionIconPosition => _interactionIconTransform.position;
        public Inventory PlayerInventory { get => _playerInventory; set => _playerInventory = value; }
        public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
        public bool IsInteracting { get => _isInteracting; set => _isInteracting = value; }
        public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
        public float CurrentLifetime { get => _currentLifetime; set => _currentLifetime = value; }
        public float DefaultLifetime => _defaultLifetime;
        public float MaxLightScale => _maxLightScale;
        public float MinLightScale => _minLightScale;
        public HeatCenter HeatCenter { get => _heatCenter; set => _heatCenter = value; }
        public InteractionRequirementsComp InteractionRequirements => _interactionRequirements;
        public BonfireFactory BonfireFactory { get => _bonfireFactory; set => _bonfireFactory = value; }
        public AudioSource KindlingBonfire => _kindlingBonfire;
        public AudioSource BurningFire => _burningFire;
        public AudioSource FireRefresh => _fireRefresh;
        public EntityFlags EntityFlags => _flags;
        public InteractionIconFactory InteractionIconFactory { get => _interactionIconFactory; set => _interactionIconFactory = value; }
        public InteractIcon InteractionIcon { get => _interactionIcon; set => _interactionIcon = value; }
    }
}
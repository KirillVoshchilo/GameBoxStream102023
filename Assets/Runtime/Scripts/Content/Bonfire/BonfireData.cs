using App.Architecture.AppInput;
using App.Content;
using App.Content.UI.WorldCanvases;
using System;
using UnityEngine;

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
    [SerializeField] private HeatCetner _heatCenter;
    [SerializeField] private AudioSource _kindlingBonfire;
    [SerializeField] private AudioSource _burningFire;
    [SerializeField] private AudioSource _fireRefresh;

    private BonfireFactory _bonfireFactory;
    private float _currentLifetime;
    private readonly InteractionComp _interactableComp = new();
    private Inventory _playerInventory;
    private WorldCanvasStorage _worldCanvasStorage;
    private IAppInputSystem _appInputSystem;
    private bool _isInteracting = false;
    private bool _isInteractable;

    public GameObject RootObject => _rootObject;
    public Alternatives[] Alternatives => _interactionRequirements.Alternatives;
    public float InteractTime => _interactTime;
    public InteractionComp InteractableComp => _interactableComp;
    public Vector3 InteractionIconPosition => _interactionIconTransform.position;
    public Inventory PlayerInventory { get => _playerInventory; set => _playerInventory = value; }
    public WorldCanvasStorage WorldCanvasStorage { get => _worldCanvasStorage; set => _worldCanvasStorage = value; }
    public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
    public bool IsInteracting { get => _isInteracting; set => _isInteracting = value; }
    public InteractIcon InteractIcon => _worldCanvasStorage.InteractIcon;
    public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
    public float CurrentLifetime { get => _currentLifetime; set => _currentLifetime = value; }
    public float DefaultLifetime => _defaultLifetime;
    public float MaxLightScale => _maxLightScale;
    public float MinLightScale => _minLightScale;
    public HeatCetner HeatCenter { get => _heatCenter; set => _heatCenter = value; }
    public InteractionRequirementsComp InteractionRequirements => _interactionRequirements;
    public BonfireFactory BonfireFactory { get => _bonfireFactory; set => _bonfireFactory = value; }
    public AudioSource KindlingBonfire => _kindlingBonfire;
    public AudioSource BurningFire => _burningFire;
    public AudioSource FireRefresh => _fireRefresh;
}
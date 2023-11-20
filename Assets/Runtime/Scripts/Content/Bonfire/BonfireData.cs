using App.Architecture.AppInput;
using App.Components;
using App.Content.Entities;
using App.Content.UI.WorldCanvases;
using App.Logic;
using System;
using UnityEngine;

[Serializable]
public class BonfireData
{
    [SerializeField] private float _defaultLifetime;
    [SerializeField] private GameObject _rootObject;
    [SerializeField] private InteractionRequirementsComp _interactionRequirements;
    [SerializeField] private Transform _interactionIconTransform;
    [SerializeField] private float _interactTime;
    [SerializeField] private float _recoverTime;
    [SerializeField] private TriggerComponent _playerSearcher;

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
    public float RecoverTime => _recoverTime;
    public bool IsInteracting { get => _isInteracting; set => _isInteracting = value; }
    public InteractIcon InteractIcon => _worldCanvasStorage.InteractIcon;
    public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
    public float CurrentLifetime { get => _currentLifetime; set => _currentLifetime = value; }
    public float DefaultLifetime => _defaultLifetime;
}

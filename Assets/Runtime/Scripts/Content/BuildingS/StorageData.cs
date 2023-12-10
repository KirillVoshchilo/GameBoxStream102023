using App.Architecture.AppInput;
using App.Content;
using App.Content.Entities;
using App.Content.UI.WorldCanvases;
using App.Logic;
using System;
using UnityEngine;

[Serializable]
public class StorageData
{
    [SerializeField] private Transform _interactionIconTransform;

    private readonly float _interactTime = 0;
    private readonly InteractionComp _interactableComp = new();
    private WorldCanvasStorage _worldCanvasStorage;
    private Inventory _playerInventory;
    private Inventory _storageInventory;
    private UIController _uiController;
    private IAppInputSystem _appInputSystem;

    public WorldCanvasStorage WorldCanvasStorage { get => _worldCanvasStorage; set => _worldCanvasStorage = value; }
    public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
    public InteractionComp InteractableComp => _interactableComp;
    public UIController UIController { get => _uiController; set => _uiController = value; }
    public Vector3 InteractionIconPosition => _interactionIconTransform.position;
    public InteractIcon InteractIcon => _worldCanvasStorage.InteractIcon;
    public float InteractTime => _interactTime;
    public Inventory PlayerInventory { get => _playerInventory; set => _playerInventory = value; }
    public Inventory StorageInventory { get => _storageInventory; set => _storageInventory = value; }
}
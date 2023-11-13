using App.Architecture.AppInput;
using App.Architecture;
using App.Content.Entities;
using App.Logic;
using UnityEngine;
using App.Architecture.AppData;
using App.Content.UI.WorldCanvases;
using System;

namespace App.Content.Field
{
    [Serializable]
    public sealed class FieldData
    {
        [SerializeField] private Key _key;
        [SerializeField] private int _itemsCount;
        [SerializeField] private GameObject _crystal;
        [SerializeField] private InteractionRequirementsComp _fieldRequirements;
        [SerializeField] private Transform _interactionIconTransform;
        [SerializeField] private float _interactTime;
        [SerializeField] private float _recoverTime;

        private readonly InteractionComp _interactableComp = new();
        private PlayerInventorySystem _playerInventory;
        private WorldCanvasStorage _worldCanvasStorage;
        private IAppInputSystem _appInputSystem;
        private bool _isRecovered = true;
        private bool _isInteracting = false;
        private bool _isInteractable;

        public Key Key => _key;
        public int ItemsCount => _itemsCount;
        public GameObject Crystal => _crystal;
        public ItemCount[] FieldRequirements => _fieldRequirements.Requirements;
        public float InteractTime => _interactTime;
        public InteractionComp InteractableComp => _interactableComp;
        public Vector3 InteractionIconPosition => _interactionIconTransform.position;
        public PlayerInventorySystem PlayerInventorySystem { get => _playerInventory; set => _playerInventory = value; }
        public WorldCanvasStorage WorldCanvasStorage { get => _worldCanvasStorage; set => _worldCanvasStorage = value; }
        public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
        public float RecoverTime => _recoverTime;
        public bool IsRecovered { get => _isRecovered; set => _isRecovered = value; }
        public bool IsInteracting { get => _isInteracting; set => _isInteracting = value; }
        public InteractIcon InteractIcon => _worldCanvasStorage.InteractIcon;
        public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
    }
}
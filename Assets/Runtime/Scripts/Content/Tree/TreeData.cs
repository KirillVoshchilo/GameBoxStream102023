using App.Architecture.AppInput;
using UnityEngine;
using App.Architecture.AppData;
using System;
using App.Simples;
using App.Simples.CellsInventory;
using App.Content.UI;

namespace App.Content.Tree
{
    [Serializable]
    public sealed class TreeData
    {
        [SerializeField] private SSOKey _key;
        [SerializeField] private int _itemsCount;
        [SerializeField] private GameObject _treeObject;
        [SerializeField] private InteractionRequirementsComp _fieldRequirements;
        [SerializeField] private Transform _interactionIconTransform;
        [SerializeField] private float _interactTime;
        [SerializeField] private float _recoverTime;
        [SerializeField] private AudioSource _fallingTreeSound;

        private readonly EntityFlags _flags = new(new string[] { Flags.TREE });
        private readonly InteractionComp _interactableComp = new();
        private Inventory _playerInventory;
        private WorldCanvasStorage _worldCanvasStorage;
        private IAppInputSystem _appInputSystem;
        private bool _isRecovered = true;
        private bool _isInteracting = false;
        private bool _isInteractable;

        public SSOKey Key => _key;
        public int ItemsCount => _itemsCount;
        public GameObject TreeObject => _treeObject;
        public Alternatives[] Alternatives => _fieldRequirements.Alternatives;
        public float InteractTime => _interactTime;
        public InteractionComp InteractableComp => _interactableComp;
        public Vector3 InteractionIconPosition => _interactionIconTransform.position;
        public Inventory PlayerInventory { get => _playerInventory; set => _playerInventory = value; }
        public WorldCanvasStorage WorldCanvasStorage { get => _worldCanvasStorage; set => _worldCanvasStorage = value; }
        public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
        public float RecoverTime => _recoverTime;
        public bool IsRecovered { get => _isRecovered; set => _isRecovered = value; }
        public bool IsInteracting { get => _isInteracting; set => _isInteracting = value; }
        public InteractIcon InteractIcon => _worldCanvasStorage.InteractIcon;
        public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
        public InteractionRequirementsComp FieldRequirements => _fieldRequirements;
        public AudioSource FallingTreeSound => _fallingTreeSound;
        public EntityFlags EntityFlags => _flags;
    }
}
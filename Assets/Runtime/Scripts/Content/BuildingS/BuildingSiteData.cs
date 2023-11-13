using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Entities;
using App.Content.UI.WorldCanvases;
using App.Logic;
using System;
using UnityEngine;

namespace App.Content.Buildings
{
    [Serializable]
    public sealed class BuildingSiteData
    {
        [SerializeField] private InteractionRequirementsComp _buildRequirements;
        [SerializeField] private Transform _requirementsPanelTransform;
        [SerializeField] private AScriptableFactory _buildingFactory;
        [SerializeField] private Transform _builderTransform;
        [Header("Interaction")]
        [SerializeField] private Transform _interactionIconTransform;

        private readonly float _interactTime = 0;
        private readonly InteractionComp _interactableComp = new();
        private bool _isInteractable;
        private IAppInputSystem _appInputSystem;
        private PlayerInventorySystem _playerInventory;
        private WorldCanvasStorage _worldCanvasStorage;

        public ItemCount[] BuildRequirements => _buildRequirements.Requirements;
        public Vector3 RequirementsPanelPosition => _requirementsPanelTransform.position;
        public AScriptableFactory BuildingFactory => _buildingFactory;
        public Transform BuilderTransform => _builderTransform;
        public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
        public PlayerInventorySystem PlayerInventory { get => _playerInventory; set => _playerInventory = value; }
        public WorldCanvasStorage WorldCanvasStorage { get => _worldCanvasStorage; set => _worldCanvasStorage = value; }
        public RequirementsPanel RequirementsPanel => _worldCanvasStorage.RequirementsPanel;
        public InteractIcon InteractIcon => _worldCanvasStorage.InteractIcon;
        public InteractionComp InteractableComp => _interactableComp;
        public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
        public Vector3 InteractionIconPosition => _interactionIconTransform.position;
        public float InteractTime => _interactTime;
    }
}
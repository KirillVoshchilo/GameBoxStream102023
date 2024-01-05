using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Architecture.Factories.UI;
using App.Content.UI;
using App.Logic;
using App.Simples.CellsInventory;
using System;
using UnityEngine;

namespace App.Content.Grigory
{
    [Serializable]
    public sealed class GrigoryData
    {
        [SerializeField] private Transform _interactionIconTransform;

        private readonly float _interactTime = 0;
        private readonly InteractionComp _interactableComp = new();
        private readonly EntityFlags _flags = new(new string[] { Flags.NPC });
        private InteractionIconFactory _interactionIconFactory;
        private Inventory _storageInventory;
        private UIController _uiController;
        private IAppInputSystem _appInputSystem;
        private Configuration _configuration;
        private InteractIcon _interactIcon;

        public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
        public InteractionComp InteractableComp => _interactableComp;
        public UIController UIController { get => _uiController; set => _uiController = value; }
        public Vector3 InteractionIconPosition => _interactionIconTransform.position;
        public float InteractTime => _interactTime;
        public Inventory StorageInventory { get => _storageInventory; set => _storageInventory = value; }
        public Configuration Configuration { get => _configuration; set => _configuration = value; }
        public EntityFlags EntityFlags => _flags;
        public InteractionIconFactory InteractionIconFactory { get => _interactionIconFactory; set => _interactionIconFactory = value; }
        public InteractIcon InteractIcon { get => _interactIcon; set => _interactIcon = value; }
    }
}
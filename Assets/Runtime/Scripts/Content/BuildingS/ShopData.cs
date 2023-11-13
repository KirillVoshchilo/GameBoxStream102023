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
    public sealed class ShopData
    {
        [SerializeField] private ItemCost[] _priceList;
        [SerializeField] private Transform _interactionIconTransform;

        private readonly float _interactTime = 0;
        private readonly InteractionComp _interactableComp = new();
        private WorldCanvasStorage _worldCanvasStorage;
        private UIController _uiController;
        private IAppInputSystem _appInputSystem;

        public ItemCost[] PriceList => _priceList;
        public WorldCanvasStorage WorldCanvasStorage { get => _worldCanvasStorage; set => _worldCanvasStorage = value; }
        public UIController UIController { get => _uiController; set => _uiController = value; }
        public InteractionComp InteractableComp => _interactableComp;
        public Vector3 InteractionIconPosition => _interactionIconTransform.position;
        public InteractIcon InteractIcon => _worldCanvasStorage.InteractIcon;
        public IAppInputSystem AppInputSystem { get => _appInputSystem; set => _appInputSystem = value; }
        public float InteractTime => _interactTime;
    }
}
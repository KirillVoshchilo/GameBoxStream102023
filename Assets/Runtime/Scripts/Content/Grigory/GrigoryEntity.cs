using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Architecture.Factories.UI;
using App.Logic;
using App.Simples;
using App.Simples.CellsInventory;
using UnityEngine;
using VContainer;

namespace App.Content.Grigory
{
    public sealed class GrigoryEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private GrigoryData _grigoryData;

        private GrigoryInteractionHandler _grigoryInteractionHandler;
        private bool _isEnable;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (value == _isEnable)
                    return;
                _isEnable = value;
                if (value)
                    _grigoryInteractionHandler.IsEnable = true;
                else _grigoryInteractionHandler.IsEnable = false;
            }
        }

        [Inject]
        public void Construct(UIController uiController,
                 InteractionIconFactory interactionIconFactory,
                 IAppInputSystem appInputSystem,
                 Configuration configuration)
        {
            _grigoryData.Configuration = configuration;
            _grigoryData.AppInputSystem = appInputSystem;
            _grigoryData.UIController = uiController;
            _grigoryData.InteractableComp.Entity = this;
            _grigoryData.InteractionIconFactory = interactionIconFactory;
            _grigoryInteractionHandler = new GrigoryInteractionHandler(_grigoryData);
        }
        public void ResetInventory()
        {
            _grigoryData.StorageInventory = new Inventory(_grigoryData.Configuration.StorageInventoryConfigurations, 9);
            FillInventory(_grigoryData.Configuration);
        }
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(InteractionComp))
                return _grigoryData.InteractableComp as T;
            if (typeof(T) == typeof(EntityFlags))
                return _grigoryData.EntityFlags as T;
            return null;
        }
        private void FillInventory(Configuration configuration)
        {
            int count = configuration.DefauleStorageItems.Items.Length;
            for (int i = 0; i < count; i++)
            {
                SSOKey key = configuration.DefauleStorageItems.Items[i].Key;
                int quantity = configuration.DefauleStorageItems.Items[i].Count;
                _grigoryData.StorageInventory.AddItem(key, quantity);
            }
        }
    }
}
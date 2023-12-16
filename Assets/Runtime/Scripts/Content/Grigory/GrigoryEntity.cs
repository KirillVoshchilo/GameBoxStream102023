using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Logic;
using App.Simples;
using App.Simples.CellsInventory;
using UnityEngine;
using VContainer;

namespace App.Content.Grigory
{
    public class GrigoryEntity : MonoBehaviour, IEntity, IDestructable
    {
        [SerializeField] private GrigoryData _grigoryData;

        [Inject]
        public void Construct(UIController uiController,
                 WorldCanvasStorage worldCanvasStorage,
                 IAppInputSystem appInputSystem,
                 Configuration configuration)
        {
            _grigoryData.Configuration = configuration;
            _grigoryData.AppInputSystem = appInputSystem;
            _grigoryData.UIController = uiController;
            _grigoryData.WorldCanvasStorage = worldCanvasStorage;
            _grigoryData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
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
            return null;
        }
        public void Destruct()
        {
            _grigoryData.InteractableComp.OnFocusChanged.RemoveListener(OnFocusChanged);
            _grigoryData.AppInputSystem.OnInteractionPerformed.ClearListeners();
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
        private void OnFocusChanged(bool obj)
        {
            if (obj)
            {
                ShowInteractionIcon();
                _grigoryData.AppInputSystem.SetInteractionTime(_grigoryData.InteractTime);
                _grigoryData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
            }
            else
            {
                CloseInteractionIcon();
                _grigoryData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnPerformedInteraction);
            }
        }
        private void CloseInteractionIcon()
        {
            _grigoryData.InteractIcon.CloseProgress();
            _grigoryData.InteractIcon.CloseTip();
            _grigoryData.InteractIcon.IsEnable = false;
            _grigoryData.InteractIcon.gameObject.SetActive(false);
        }
        private void ShowInteractionIcon()
        {
            _grigoryData.InteractIcon.SetPosition(_grigoryData.InteractionIconPosition);
            _grigoryData.InteractIcon.gameObject.SetActive(true);
            _grigoryData.InteractIcon.IsEnable = true;
            _grigoryData.InteractIcon.OpenTip();
            _grigoryData.InteractIcon.HoldMode = false;
        }
        private void OnPerformedInteraction()
            => _grigoryData.UIController.OpenStorageMenu(_grigoryData.StorageInventory);
    }
}
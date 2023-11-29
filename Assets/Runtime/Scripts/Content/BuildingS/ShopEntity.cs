using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Entities;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Content.Buildings
{
    public sealed class ShopEntity : MonoBehaviour, IEntity, IDestructable
    {
        [SerializeField] private ShopData _shopData;

        [Inject]
        public void Construct(UIController uiController,
            WorldCanvasStorage worldCanvasStorage,
            IAppInputSystem appInputSystem)
        {
            _shopData.AppInputSystem = appInputSystem;
            _shopData.UIController = uiController;
            _shopData.WorldCanvasStorage = worldCanvasStorage;
            _shopData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
            Debug.Log("Сконструировал ShopEntity.");
        }
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(InteractionComp))
                return _shopData.InteractableComp as T;
            return null;
        }
        public void Destruct()
        {
            _shopData.InteractableComp.OnFocusChanged.RemoveListener(OnFocusChanged);
            _shopData.AppInputSystem.OnInteractionPerformed.ClearListeners();
        }

        private void OnFocusChanged(bool obj)
        {
            if (obj)
            {
                ShowInteractionIcon();
                _shopData.AppInputSystem.SetInteractionTime(_shopData.InteractTime);
                _shopData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
            }
            else
            {
                CloseInteractionIcon();
                _shopData.AppInputSystem.OnInteractionPerformed.ClearListeners();
            }
        }
        private void CloseInteractionIcon()
        {
            _shopData.InteractIcon.CloseProgress();
            _shopData.InteractIcon.CloseTip();
            _shopData.InteractIcon.IsEnable = false;
            _shopData.InteractIcon.gameObject.SetActive(false);
        }
        private void ShowInteractionIcon()
        {
            _shopData.InteractIcon.SetPosition(_shopData.InteractionIconPosition);
            _shopData.InteractIcon.gameObject.SetActive(true);
            _shopData.InteractIcon.IsEnable = true;
            _shopData.InteractIcon.OpenTip();
            _shopData.InteractIcon.HoldMode = false;
        }
        private void OnPerformedInteraction()
        {
        //    _shopData.UIController.OpenShop(_shopData.PriceList);
        } 
    }
}
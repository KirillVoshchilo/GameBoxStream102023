using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Content.Fevronia
{
    public sealed class FevroniaEntity : MonoBehaviour, IEntity, IDestructable
    {
        [SerializeField] private FevroniaData _fevroniaData;

        [Inject]
        public void Construct(UIController uiController,
              WorldCanvasStorage worldCanvasStorage,
              IAppInputSystem appInputSystem)
        {
            _fevroniaData.AppInputSystem = appInputSystem;
            _fevroniaData.UIController = uiController;
            _fevroniaData.WorldCanvasStorage = worldCanvasStorage;
            _fevroniaData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
        }
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(InteractionComp))
                return _fevroniaData.InteractableComp as T;
            return null;
        }
        public void Destruct()
        {
            _fevroniaData.InteractableComp.OnFocusChanged.RemoveListener(OnFocusChanged);
            _fevroniaData.AppInputSystem.OnInteractionPerformed.ClearListeners();
        }

        private void OnFocusChanged(bool obj)
        {
            if (obj)
            {
                ShowInteractionIcon();
                _fevroniaData.AppInputSystem.SetInteractionTime(_fevroniaData.InteractTime);
                _fevroniaData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
            }
            else
            {
                CloseInteractionIcon();
                _fevroniaData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnPerformedInteraction);
            }
        }
        private void CloseInteractionIcon()
        {
            _fevroniaData.InteractIcon.CloseProgress();
            _fevroniaData.InteractIcon.CloseTip();
            _fevroniaData.InteractIcon.IsEnable = false;
            _fevroniaData.InteractIcon.gameObject.SetActive(false);
        }
        private void ShowInteractionIcon()
        {
            _fevroniaData.InteractIcon.SetPosition(_fevroniaData.InteractionIconPosition);
            _fevroniaData.InteractIcon.gameObject.SetActive(true);
            _fevroniaData.InteractIcon.IsEnable = true;
            _fevroniaData.InteractIcon.OpenTip();
            _fevroniaData.InteractIcon.HoldMode = false;
        }
        private void OnPerformedInteraction()
            => _fevroniaData.UIController.OpenScarecrowMenu();
    }
}
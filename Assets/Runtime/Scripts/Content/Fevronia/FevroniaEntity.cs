using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Architecture.Factories.UI;
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
              InteractionIconFactory interactionIconFactory,
              IAppInputSystem appInputSystem)
        {
            _fevroniaData.InteractionIconFactory = interactionIconFactory;
            _fevroniaData.AppInputSystem = appInputSystem;
            _fevroniaData.UIController = uiController;
            _fevroniaData.InteractableComp.Entity = this;
            _fevroniaData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
        }
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(InteractionComp))
                return _fevroniaData.InteractableComp as T;
            if (typeof(T) == typeof(EntityFlags))
                return _fevroniaData.EntityFlags as T;
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
            if (_fevroniaData.InteractIcon == null)
                return;
            _fevroniaData.InteractionIconFactory.Remove(_fevroniaData.InteractIcon);
        }
        private void ShowInteractionIcon()
        {
            _fevroniaData.InteractIcon = _fevroniaData.InteractionIconFactory.Create();
            _fevroniaData.InteractIcon.SetPosition(_fevroniaData.InteractionIconPosition);
            _fevroniaData.InteractIcon.IsEnable = true;
            _fevroniaData.InteractIcon.OpenTip();
            _fevroniaData.InteractIcon.HoldMode = false;
        }
        private void OnPerformedInteraction()
            => _fevroniaData.UIController.OpenFevroniaMenu();
    }
}
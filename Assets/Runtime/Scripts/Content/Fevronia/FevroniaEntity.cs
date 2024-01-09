using App.Architecture.AppInput;
using App.Architecture.Factories.UI;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Content.Fevronia
{
    public sealed class FevroniaEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private FevroniaData _fevroniaData;

        private FevroniaInteractionHandler _fevroniaInteractionHandler;
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
                    _fevroniaInteractionHandler.IsEnable = true;
                else _fevroniaInteractionHandler.IsEnable = false;
            }
        }

        [Inject]
        public void Construct(UIController uiController,
              InteractionIconFactory interactionIconFactory,
              IAppInputSystem appInputSystem)
        {
            _fevroniaData.InteractionIconFactory = interactionIconFactory;
            _fevroniaData.AppInputSystem = appInputSystem;
            _fevroniaData.UIController = uiController;
            _fevroniaData.InteractableComp.Entity = this;
            _fevroniaInteractionHandler = new FevroniaInteractionHandler(_fevroniaData);
        }
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(InteractionComp))
                return _fevroniaData.InteractableComp as T;
            if (typeof(T) == typeof(EntityFlags))
                return _fevroniaData.EntityFlags as T;
            return null;
        }
    }
}
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Architecture.Factories.UI;
using App.Simples.CellsInventory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Content.Bonfire
{
    public sealed class BonfireEntity : MonoBehaviour, IEntity, IDestructable
    {
        [SerializeField] private BonfireData _bonfireData;

        public void Construct(InteractionIconFactory interactionIconFactory,
                  IAppInputSystem appInputSystem,
                  Inventory playerInventory,
                  BonfireFactory bonfireFactory)
        {
            _bonfireData.BonfireFactory = bonfireFactory;
            _bonfireData.PlayerInventory = playerInventory;
            _bonfireData.AppInputSystem = appInputSystem;
            _bonfireData.InteractionIconFactory = interactionIconFactory;
            _bonfireData.InteractableComp.Entity = this;
            _bonfireData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
            _bonfireData.CurrentLifetime = _bonfireData.DefaultLifetime;
            _bonfireData.BurningFire.Play();
            _bonfireData.KindlingBonfire.Play();
            _bonfireData.HeatCenter.Construct();
            ExtinguishingProcess()
                .Forget();
        }
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(InteractionComp))
                return _bonfireData.InteractableComp as T;
            if (typeof(T) == typeof(InteractionRequirementsComp))
                return _bonfireData.InteractionRequirements as T;
            if (typeof(T) == typeof(EntityFlags))
                return _bonfireData.EntityFlags as T;
            return null;
        }
        public void Destruct()
        {
            _bonfireData.CurrentLifetime = 0;
            _bonfireData.InteractableComp.OnFocusChanged.RemoveListener(OnFocusChanged);
            _bonfireData.AppInputSystem.OnInteractionStarted.RemoveListener(OnStartedInteracrtion);
            _bonfireData.AppInputSystem.OnInteractionCanceled.RemoveListener(OnCancelInteraction);
            _bonfireData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnPerformedInteraction);
            _bonfireData.HeatCenter.Destruct();
        }

        private async UniTask ExtinguishingProcess()
        {
            while (_bonfireData.CurrentLifetime > 0)
            {
                _bonfireData.CurrentLifetime -= 1 * Time.deltaTime;
                UpdateLightView();
                await UniTask.NextFrame();
            }
            _bonfireData.BonfireFactory.Remove(this);
        }
        private void UpdateLightView()
        {
            float currentScale = _bonfireData.MinLightScale + (_bonfireData.CurrentLifetime / _bonfireData.DefaultLifetime * (_bonfireData.MaxLightScale - _bonfireData.MinLightScale));
            _bonfireData.HeatCenter.HeatZone.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
        private void OnFocusChanged(bool value)
        {
            CheckInteractable();
            if (value && _bonfireData.IsInteractable)
            {
                ShowInteractionIcon();
                EnableInteraction();
            }
            else
            {
                CloseInteractionIcon();
                DisableInteraction();
            }
        }
        private void CheckInteractable()
        {
            foreach (Alternatives alternative in _bonfireData.Alternatives)
            {
                if (CheckRequirements(alternative))
                {
                    _bonfireData.IsInteractable = true;
                    return;
                }
            }
            _bonfireData.IsInteractable = false;
        }
        private bool CheckRequirements(Alternatives alternative)
        {
            foreach (ItemCount item in alternative.Requirements)
            {
                if (_bonfireData.PlayerInventory.GetCount(item.Key) < item.Count)
                    return false;
            }
            return true;
        }
        private void DisableInteraction()
        {
            _bonfireData.AppInputSystem.OnInteractionStarted.RemoveListener(OnStartedInteracrtion);
            _bonfireData.AppInputSystem.OnInteractionCanceled.RemoveListener(OnCancelInteraction);
            _bonfireData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnPerformedInteraction);
        }
        private void EnableInteraction()
        {
            _bonfireData.AppInputSystem.SetInteractionTime(_bonfireData.InteractTime);
            _bonfireData.AppInputSystem.OnInteractionStarted.AddListener(OnStartedInteracrtion);
            _bonfireData.AppInputSystem.OnInteractionCanceled.AddListener(OnCancelInteraction);
            _bonfireData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
        }
        private void CloseInteractionIcon()
        {
            _bonfireData.InteractionIconFactory.Remove(_bonfireData.InteractionIcon);
        }
        private void ShowInteractionIcon()
        {
            _bonfireData.InteractionIcon = _bonfireData.InteractionIconFactory.Create();
            _bonfireData.InteractionIcon.SetPosition(_bonfireData.InteractionIconPosition);
            _bonfireData.InteractionIcon.gameObject.SetActive(true);
            _bonfireData.InteractionIcon.IsEnable = true;
            _bonfireData.InteractionIcon.OpenTip();
            _bonfireData.InteractionIcon.HoldMode = true;
        }
        private void OnPerformedInteraction()
        {
            foreach (ItemCount item in _bonfireData.Alternatives[0].Requirements)
                _bonfireData.PlayerInventory.RemoveItem(item.Key, item.Count);
            _bonfireData.CurrentLifetime = _bonfireData.DefaultLifetime;
            _bonfireData.FireRefresh.Play();
            CloseInteractionIcon();
            DisableInteraction();
        }
        private void OnCancelInteraction()
        {
            _bonfireData.InteractionIcon.CloseProgress();
            _bonfireData.InteractionIcon.OpenTip();
            _bonfireData.IsInteracting = false;
            _bonfireData.InteractionIcon.HoldMode = true;
        }
        private void OnStartedInteracrtion()
        {
            _bonfireData.IsInteracting = true;
            _bonfireData.InteractionIcon.CloseTip();
            _bonfireData.InteractionIcon.OpenProgress();
            ProgressVisualize()
                .Forget();
        }
        private async UniTask ProgressVisualize()
        {
            while (_bonfireData.IsInteracting)
            {
                _bonfireData.InteractionIcon.SetProgress(_bonfireData.AppInputSystem.InteractionPercentage);
                await UniTask.Delay(50);
            }
        }
    }
}
﻿using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Entities;
using App.Content.Player;
using App.Logic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace App.Content.Field
{
    public sealed class ResourceSourceEntity : MonoBehaviour, IEntity, IDestructable
    {
        [SerializeField] private ResourceSourceData _resourceSourceData;

        [Inject]
        public void Construct(WorldCanvasStorage worldCanvasStorage,
            IAppInputSystem appInputSystem,
            PlayerEntity playerEntity)
        {
            _resourceSourceData.PlayerInventory = playerEntity.Get<Inventory>();
            _resourceSourceData.AppInputSystem = appInputSystem;
            _resourceSourceData.WorldCanvasStorage = worldCanvasStorage;
            _resourceSourceData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
        }
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(InteractionComp))
                return _resourceSourceData.InteractableComp as T;
            if (typeof(T) == typeof(InteractionRequirementsComp))
                return _resourceSourceData as T;
            return null;
        }
        public void Destruct()
        {
            _resourceSourceData.InteractableComp.OnFocusChanged.ClearListeners();
            _resourceSourceData.AppInputSystem.OnInteractionStarted.ClearListeners();
            _resourceSourceData.AppInputSystem.OnInteractionCanceled.ClearListeners();
            _resourceSourceData.AppInputSystem.OnInteractionPerformed.ClearListeners();
        }

        private void OnFocusChanged(bool value)
        {
            CheckInteractable();
            if (value && _resourceSourceData.IsRecovered && _resourceSourceData.IsInteractable)
            {
                ShowInteractionIcon();
                EnableInteraction();
                Debug.Log($"FieldEntity OnFocusChanged {value}.");
            }
            else
            {
                CloseInteractionIcon();
                DisableInteraction();
                Debug.Log($"FieldEntity OnFocusChanged {value}.");
            }
        }
        private void CheckInteractable()
        {
            foreach (Alternatives alternative in _resourceSourceData.Alternatives)
            {
                if (CheckRequirements(alternative))
                {
                    _resourceSourceData.IsInteractable = true;
                    return;
                }
            }
            _resourceSourceData.IsInteractable = false;
        }
        private bool CheckRequirements(Alternatives alternative)
        {
            foreach (ItemCount item in alternative.Requirements)
            {
                if (_resourceSourceData.PlayerInventory.GetCount(item.Key) < item.Count)
                    return false;
            }
            return true;
        }
        private void DisableInteraction()
        {
            _resourceSourceData.AppInputSystem.OnInteractionStarted.ClearListeners();
            _resourceSourceData.AppInputSystem.OnInteractionCanceled.ClearListeners();
            _resourceSourceData.AppInputSystem.OnInteractionPerformed.ClearListeners();
        }
        private void EnableInteraction()
        {
            _resourceSourceData.AppInputSystem.SetInteractionTime(_resourceSourceData.InteractTime);
            _resourceSourceData.AppInputSystem.OnInteractionStarted.AddListener(OnStartedInteracrtion);
            _resourceSourceData.AppInputSystem.OnInteractionCanceled.AddListener(OnCancelInteraction);
            _resourceSourceData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
        }
        private void CloseInteractionIcon()
        {
            _resourceSourceData.InteractIcon.CloseProgress();
            _resourceSourceData.InteractIcon.CloseTip();
            _resourceSourceData.InteractIcon.IsEnable = false;
            _resourceSourceData.InteractIcon.gameObject.SetActive(false);
        }
        private void ShowInteractionIcon()
        {
            _resourceSourceData.InteractIcon.SetPosition(_resourceSourceData.InteractionIconPosition);
            _resourceSourceData.InteractIcon.gameObject.SetActive(true);
            _resourceSourceData.InteractIcon.IsEnable = true;
            _resourceSourceData.InteractIcon.OpenTip();
            _resourceSourceData.InteractIcon.HoldMode = true;
        }
        private void OnPerformedInteraction()
        {
            _resourceSourceData.Crystal.SetActive(false);
            _resourceSourceData.PlayerInventory.AddItem(_resourceSourceData.Key, _resourceSourceData.ItemsCount);
            CloseInteractionIcon();
            DisableInteraction();
            _resourceSourceData.IsRecovered = false;
            Recover()
                .Forget();
        }
        private void OnCancelInteraction()
        {
            _resourceSourceData.InteractIcon.CloseProgress();
            _resourceSourceData.InteractIcon.OpenTip();
            _resourceSourceData.IsInteracting = false;
            _resourceSourceData.InteractIcon.HoldMode = true;
        }
        private void OnStartedInteracrtion()
        {
            _resourceSourceData.IsInteracting = true;
            _resourceSourceData.InteractIcon.CloseTip();
            _resourceSourceData.InteractIcon.OpenProgress();
            ProgressVisualize()
                .Forget();
        }
        private async UniTask ProgressVisualize()
        {
            while (_resourceSourceData.IsInteracting)
            {
                _resourceSourceData.InteractIcon.SetProgress(_resourceSourceData.AppInputSystem.InteractionPercentage);
                await UniTask.Delay(50);
            }
        }
        private async UniTask Recover()
        {
            if (_resourceSourceData.RecoverTime < 0)
                return;
            int delay = (int)(_resourceSourceData.RecoverTime * 1000);
            await UniTask.Delay(delay);
            if (_resourceSourceData == null)
                return;
            _resourceSourceData.IsRecovered = true;
            _resourceSourceData.Crystal.SetActive(true);
            if (_resourceSourceData.InteractableComp.IsInFocus)
            {
                ShowInteractionIcon();
                EnableInteraction();
            }
        }
    }
}
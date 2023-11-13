using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Entities;
using App.Logic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace App.Content.Field
{
    public sealed class FieldEntity : MonoBehaviour, IEntity, IDestructable
    {
        [SerializeField] private FieldData _fieldData;

        [Inject]
        public void Construct(PlayerInventorySystem playerInventorySystem,
            WorldCanvasStorage worldCanvasStorage,
            IAppInputSystem appInputSystem)
        {
            _fieldData.AppInputSystem = appInputSystem;
            _fieldData.PlayerInventorySystem = playerInventorySystem;
            _fieldData.WorldCanvasStorage = worldCanvasStorage;
            _fieldData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
            Debug.Log("Сконструировал FieldEntity.");
        }
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(InteractionComp))
                return _fieldData.InteractableComp as T;
            if (typeof(T) == typeof(InteractionRequirementsComp))
                return _fieldData as T;
            return null;
        }
        public void Destruct()
        {
            _fieldData.InteractableComp.OnFocusChanged.ClearListeners();
            _fieldData.AppInputSystem.OnInteractionStarted.ClearListeners();
            _fieldData.AppInputSystem.OnInteractionCanceled.ClearListeners();
            _fieldData.AppInputSystem.OnInteractionPerformed.ClearListeners();
        }

        private void OnFocusChanged(bool value)
        {
            CheckInteractable();
            if (value && _fieldData.IsRecovered && _fieldData.IsInteractable)
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
            foreach (ItemCount item in _fieldData.FieldRequirements)
            {
                if (_fieldData.PlayerInventorySystem.GetCount(item.Key) < item.Count)
                {
                    _fieldData.IsInteractable = false;
                    return;
                }
            }
            _fieldData.IsInteractable = true;
        }
        private void DisableInteraction()
        {
            _fieldData.AppInputSystem.OnInteractionStarted.ClearListeners();
            _fieldData.AppInputSystem.OnInteractionCanceled.ClearListeners();
            _fieldData.AppInputSystem.OnInteractionPerformed.ClearListeners();
        }
        private void EnableInteraction()
        {
            _fieldData.AppInputSystem.SetInteractionTime(_fieldData.InteractTime);
            _fieldData.AppInputSystem.OnInteractionStarted.AddListener(OnStartedInteracrtion);
            _fieldData.AppInputSystem.OnInteractionCanceled.AddListener(OnCancelInteraction);
            _fieldData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
        }
        private void CloseInteractionIcon()
        {
            _fieldData.InteractIcon.CloseProgress();
            _fieldData.InteractIcon.CloseTip();
            _fieldData.InteractIcon.IsEnable = false;
            _fieldData.InteractIcon.gameObject.SetActive(false);
        }
        private void ShowInteractionIcon()
        {
            _fieldData.InteractIcon.SetPosition(_fieldData.InteractionIconPosition);
            _fieldData.InteractIcon.gameObject.SetActive(true);
            _fieldData.InteractIcon.IsEnable = true;
            _fieldData.InteractIcon.OpenTip();
            _fieldData.InteractIcon.HoldMode = true;
        }
        private void OnPerformedInteraction()
        {
            _fieldData.Crystal.SetActive(false);
            _fieldData.PlayerInventorySystem.AddItem(_fieldData.Key, _fieldData.ItemsCount);
            CloseInteractionIcon();
            DisableInteraction();
            _fieldData.IsRecovered = false;
            Recover()
                .Forget();
        }
        private void OnCancelInteraction()
        {
            _fieldData.InteractIcon.CloseProgress();
            _fieldData.InteractIcon.OpenTip();
            _fieldData.IsInteracting = false;
            _fieldData.InteractIcon.HoldMode = true;
        }
        private void OnStartedInteracrtion()
        {
            _fieldData.IsInteracting = true;
            _fieldData.InteractIcon.CloseTip();
            _fieldData.InteractIcon.OpenProgress();
            ProgressVisualize()
                .Forget();
        }
        private async UniTask ProgressVisualize()
        {
            while (_fieldData.IsInteracting)
            {
                _fieldData.InteractIcon.SetProgress(_fieldData.AppInputSystem.InteractionPercentage);
                await UniTask.Delay(50);
            }
        }
        private async UniTask Recover()
        {
            int delay = (int)(_fieldData.RecoverTime * 1000);
            await UniTask.Delay(delay);
            if (_fieldData == null)
                return;
            _fieldData.IsRecovered = true;
            _fieldData.Crystal.SetActive(true);
            if (_fieldData.InteractableComp.IsInFocus)
            {
                ShowInteractionIcon();
                EnableInteraction();
            }
        }
    }
}
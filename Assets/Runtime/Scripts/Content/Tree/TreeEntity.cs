using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Player;
using App.Simples.CellsInventory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace App.Content.Tree
{
    public sealed class TreeEntity : MonoBehaviour, IEntity, IDestructable
    {
        [SerializeField] private TreeData _treeData;

        [Inject]
        public void Construct(WorldCanvasStorage worldCanvasStorage,
            IAppInputSystem appInputSystem,
            PlayerEntity playerEntity)
        {
            _treeData.PlayerInventory = playerEntity.Get<Inventory>();
            _treeData.AppInputSystem = appInputSystem;
            _treeData.WorldCanvasStorage = worldCanvasStorage;
            _treeData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
            _treeData.InteractableComp.Transform = _treeData.TreeObject.transform;
            _treeData.InteractableComp.Entity = this;
        }
        public void Recover()
        {
            _treeData.InteractableComp.IsBlocked = false;
            _treeData.TreeObject.SetActive(true);
            _treeData.IsRecovered = true;
        }
        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(InteractionComp))
                return _treeData.InteractableComp as T;
            if (typeof(T) == typeof(InteractionRequirementsComp))
                return _treeData.FieldRequirements as T;
            if (typeof(T) == typeof(EntityFlags))
                return _treeData.EntityFlags as T;
            return null;
        }
        public void Destruct()
        {
            _treeData.InteractableComp.OnFocusChanged.RemoveListener(OnFocusChanged);
            _treeData.AppInputSystem.OnInteractionStarted.RemoveListener(OnStartedInteracrtion);
            _treeData.AppInputSystem.OnInteractionCanceled.RemoveListener(OnCancelInteraction);
            _treeData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnPerformedInteraction);
        }

        private void OnFocusChanged(bool value)
        {
            if (!_treeData.AppInputSystem.InteractionIsEnable)
                return;
            CheckInteractable();
            _treeData.InteractableComp.IsInteractable = _treeData.IsInteractable;
            if (value && _treeData.IsRecovered && _treeData.IsInteractable)
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
            foreach (Alternatives alternative in _treeData.Alternatives)
            {
                if (CheckRequirements(alternative))
                {
                    _treeData.IsInteractable = true;
                    return;
                }
            }
            _treeData.IsInteractable = false;
        }
        private bool CheckRequirements(Alternatives alternative)
        {
            foreach (ItemCount item in alternative.Requirements)
            {
                if (_treeData.PlayerInventory.GetCount(item.Key) < item.Count)
                    return false;
            }
            return true;
        }
        private void DisableInteraction()
        {
            _treeData.AppInputSystem.OnInteractionStarted.RemoveListener(OnStartedInteracrtion);
            _treeData.AppInputSystem.OnInteractionCanceled.RemoveListener(OnCancelInteraction);
            _treeData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnPerformedInteraction);
        }
        private void EnableInteraction()
        {
            _treeData.AppInputSystem.SetInteractionTime(_treeData.InteractTime);
            _treeData.AppInputSystem.OnInteractionStarted.AddListener(OnStartedInteracrtion);
            _treeData.AppInputSystem.OnInteractionCanceled.AddListener(OnCancelInteraction);
            _treeData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
        }
        private void CloseInteractionIcon()
        {
            _treeData.InteractIcon.CloseProgress();
            _treeData.InteractIcon.CloseTip();
            _treeData.InteractIcon.IsEnable = false;
            _treeData.InteractIcon.gameObject.SetActive(false);
        }
        private void ShowInteractionIcon()
        {
            _treeData.InteractIcon.SetPosition(_treeData.InteractionIconPosition);
            _treeData.InteractIcon.gameObject.SetActive(true);
            _treeData.InteractIcon.IsEnable = true;
            _treeData.InteractIcon.OpenTip();
            _treeData.InteractIcon.HoldMode = true;
        }
        private void OnPerformedInteraction()
        {
            _treeData.InteractableComp.IsBlocked = true;
            _treeData.AppInputSystem.PlayerMovingIsEnable = true;
            _treeData.TreeObject.SetActive(false);
            _treeData.PlayerInventory.AddItem(_treeData.Key, _treeData.ItemsCount);
            CloseInteractionIcon();
            DisableInteraction();
            _treeData.FallingTreeSound.Play();
            _treeData.InteractableComp.IsInFocus = false;
            _treeData.IsRecovered = false;
            DefferedRecover()
                .Forget();
        }
        private void OnCancelInteraction()
        {
            _treeData.AppInputSystem.PlayerMovingIsEnable = true;
            _treeData.InteractIcon.CloseProgress();
            _treeData.InteractIcon.OpenTip();
            _treeData.IsInteracting = false;
            _treeData.InteractIcon.HoldMode = true;
        }
        private void OnStartedInteracrtion()
        {
            _treeData.AppInputSystem.PlayerMovingIsEnable = false;
            _treeData.IsInteracting = true;
            _treeData.InteractIcon.CloseTip();
            _treeData.InteractIcon.OpenProgress();
            ProgressVisualize()
                .Forget();
        }
        private async UniTask ProgressVisualize()
        {
            while (_treeData.IsInteracting)
            {
                _treeData.InteractIcon.SetProgress(_treeData.AppInputSystem.InteractionPercentage);
                await UniTask.Delay(50);
            }
        }
        private async UniTask DefferedRecover()
        {
            if (_treeData.RecoverTime < 0)
                return;
            int delay = (int)(_treeData.RecoverTime * 1000);
            await UniTask.Delay(delay);
            if (_treeData == null)
                return;
            _treeData.IsRecovered = true;
            _treeData.TreeObject.SetActive(true);
            if (_treeData.InteractableComp.IsInFocus)
            {
                ShowInteractionIcon();
                EnableInteraction();
            }
        }
    }
}
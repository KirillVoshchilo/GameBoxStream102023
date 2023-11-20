using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Entities;
using App.Content.Player;
using App.Logic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BonfireEntity : MonoBehaviour, IEntity, IDestructable
{
    [SerializeField] private BonfireData _bonfireData;

    public void Construct(WorldCanvasStorage worldCanvasStorage,
              IAppInputSystem appInputSystem,
              PlayerEntity playerEntity)
    {
        _bonfireData.PlayerInventory = playerEntity.Get<Inventory>();
        _bonfireData.AppInputSystem = appInputSystem;
        _bonfireData.WorldCanvasStorage = worldCanvasStorage;
        _bonfireData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
        ExtinguishingProcess()
            .Forget();
        Debug.Log("Сконструировал FieldEntity.");
    }
    public T Get<T>() where T : class
    {
        if (typeof(T) == typeof(InteractionComp))
            return _bonfireData.InteractableComp as T;
        if (typeof(T) == typeof(InteractionRequirementsComp))
            return _bonfireData as T;
        return null;
    }
    public void Destruct()
    {
        _bonfireData.InteractableComp.OnFocusChanged.ClearListeners();
        _bonfireData.AppInputSystem.OnInteractionStarted.ClearListeners();
        _bonfireData.AppInputSystem.OnInteractionCanceled.ClearListeners();
        _bonfireData.AppInputSystem.OnInteractionPerformed.ClearListeners();
    }


    private async UniTask ExtinguishingProcess()
    {
        while (_bonfireData.CurrentLifetime > 0)
        {
            await UniTask.Delay(1000);
            _bonfireData.CurrentLifetime--;
        }
    }
    private void OnFocusChanged(bool value)
    {
        CheckInteractable();
        if (value && _bonfireData.IsInteractable)
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
        _bonfireData.AppInputSystem.OnInteractionStarted.ClearListeners();
        _bonfireData.AppInputSystem.OnInteractionCanceled.ClearListeners();
        _bonfireData.AppInputSystem.OnInteractionPerformed.ClearListeners();
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
        _bonfireData.InteractIcon.CloseProgress();
        _bonfireData.InteractIcon.CloseTip();
        _bonfireData.InteractIcon.IsEnable = false;
        _bonfireData.InteractIcon.gameObject.SetActive(false);
    }
    private void ShowInteractionIcon()
    {
        _bonfireData.InteractIcon.SetPosition(_bonfireData.InteractionIconPosition);
        _bonfireData.InteractIcon.gameObject.SetActive(true);
        _bonfireData.InteractIcon.IsEnable = true;
        _bonfireData.InteractIcon.OpenTip();
        _bonfireData.InteractIcon.HoldMode = true;
    }
    private void OnPerformedInteraction()
    {
        _bonfireData.RootObject.SetActive(false);
        foreach (ItemCount item in _bonfireData.Alternatives[0].Requirements)
            _bonfireData.PlayerInventory.RemoveItem(item.Key, item.Count);
        _bonfireData.CurrentLifetime = _bonfireData.DefaultLifetime;
        CloseInteractionIcon();
        DisableInteraction();
    }
    private void OnCancelInteraction()
    {
        _bonfireData.InteractIcon.CloseProgress();
        _bonfireData.InteractIcon.OpenTip();
        _bonfireData.IsInteracting = false;
        _bonfireData.InteractIcon.HoldMode = true;
    }
    private void OnStartedInteracrtion()
    {
        _bonfireData.IsInteracting = true;
        _bonfireData.InteractIcon.CloseTip();
        _bonfireData.InteractIcon.OpenProgress();
        ProgressVisualize()
            .Forget();
    }
    private async UniTask ProgressVisualize()
    {
        while (_bonfireData.IsInteracting)
        {
            _bonfireData.InteractIcon.SetProgress(_bonfireData.AppInputSystem.InteractionPercentage);
            await UniTask.Delay(50);
        }
    }
}

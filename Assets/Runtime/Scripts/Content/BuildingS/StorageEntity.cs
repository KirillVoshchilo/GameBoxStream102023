using App.Architecture.AppInput;
using App.Content.Entities;
using App.Logic;
using UnityEngine;
using VContainer;

public class StorageEntity : MonoBehaviour
{
    [SerializeField] private StorageData _storageData;

    [Inject]
    public void Construct(UIController uiController,
             WorldCanvasStorage worldCanvasStorage,
             IAppInputSystem appInputSystem)
    {
        _storageData.AppInputSystem = appInputSystem;
        _storageData.UIController = uiController;
        _storageData.WorldCanvasStorage = worldCanvasStorage;
        _storageData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
        Debug.Log("Сконструировал ShopEntity.");
    }
    public T Get<T>() where T : class
    {
        if (typeof(T) == typeof(InteractionComp))
            return _storageData.InteractableComp as T;
        return null;
    }
    public void Destruct()
    {
        _storageData.InteractableComp.OnFocusChanged.RemoveListener(OnFocusChanged);
        _storageData.AppInputSystem.OnInteractionPerformed.ClearListeners();
    }

    private void OnFocusChanged(bool obj)
    {
        if (obj)
        {
            ShowInteractionIcon();
            _storageData.AppInputSystem.SetInteractionTime(_storageData.InteractTime);
            _storageData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
        }
        else
        {
            CloseInteractionIcon();
            _storageData.AppInputSystem.OnInteractionPerformed.ClearListeners();
        }
    }
    private void CloseInteractionIcon()
    {
        _storageData.InteractIcon.CloseProgress();
        _storageData.InteractIcon.CloseTip();
        _storageData.InteractIcon.IsEnable = false;
        _storageData.InteractIcon.gameObject.SetActive(false);
    }
    private void ShowInteractionIcon()
    {
        _storageData.InteractIcon.SetPosition(_storageData.InteractionIconPosition);
        _storageData.InteractIcon.gameObject.SetActive(true);
        _storageData.InteractIcon.IsEnable = true;
        _storageData.InteractIcon.OpenTip();
        _storageData.InteractIcon.HoldMode = false;
    }
    private void OnPerformedInteraction()
        => _storageData.UIController.OpenStorageMenu();
}

using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Entities;
using App.Logic;
using UnityEngine;
using VContainer;

public sealed class ScarecrowEntity : MonoBehaviour, IEntity, IDestructable
{
    [SerializeField] private ScarecrowData _scarecrowData;

    [Inject]
    public void Construct(UIController uiController,
          WorldCanvasStorage worldCanvasStorage,
          IAppInputSystem appInputSystem)
    {
        _scarecrowData.AppInputSystem = appInputSystem;
        _scarecrowData.UIController = uiController;
        _scarecrowData.WorldCanvasStorage = worldCanvasStorage;
        _scarecrowData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
        Debug.Log("Сконструировал ShopEntity.");
    }
    public T Get<T>() where T : class
    {
        if (typeof(T) == typeof(InteractionComp))
            return _scarecrowData.InteractableComp as T;
        return null;
    }
    public void Destruct()
    {
        _scarecrowData.InteractableComp.OnFocusChanged.RemoveListener(OnFocusChanged);
        _scarecrowData.AppInputSystem.OnInteractionPerformed.ClearListeners();
    }

    private void OnFocusChanged(bool obj)
    {
        if (obj)
        {
            ShowInteractionIcon();
            _scarecrowData.AppInputSystem.SetInteractionTime(_scarecrowData.InteractTime);
            _scarecrowData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
        }
        else
        {
            CloseInteractionIcon();
            _scarecrowData.AppInputSystem.OnInteractionPerformed.ClearListeners();
        }
    }
    private void CloseInteractionIcon()
    {
        _scarecrowData.InteractIcon.CloseProgress();
        _scarecrowData.InteractIcon.CloseTip();
        _scarecrowData.InteractIcon.IsEnable = false;
        _scarecrowData.InteractIcon.gameObject.SetActive(false);
    }
    private void ShowInteractionIcon()
    {
        _scarecrowData.InteractIcon.SetPosition(_scarecrowData.InteractionIconPosition);
        _scarecrowData.InteractIcon.gameObject.SetActive(true);
        _scarecrowData.InteractIcon.IsEnable = true;
        _scarecrowData.InteractIcon.OpenTip();
        _scarecrowData.InteractIcon.HoldMode = false;
    }
    private void OnPerformedInteraction()
        => _scarecrowData.UIController.OpenScarecrowMenu();
}

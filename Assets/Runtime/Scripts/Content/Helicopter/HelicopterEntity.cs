using App.Content.Player;
using UnityEngine;
using VContainer;

public class HelicopterEntity : MonoBehaviour
{
    [SerializeField] private HelicopterData _helicopterData;

    [Inject]
    public void Construct(FinishController finishController)
    {
        _helicopterData.FinishController = finishController;
        _helicopterData.TriggerComponent.OnEnter.AddListener(OnTrigger);
    }

    private void OnTrigger(Collider other)
    {
        if (other.TryGetComponent(out PlayerEntity playerEntity))
            _helicopterData.FinishController.EscapeFinish();
    }
}

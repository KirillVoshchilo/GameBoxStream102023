using App.Content.Player;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Content.Helicopter
{
    public sealed class HelicopterEntity : MonoBehaviour
    {
        [SerializeField] private HelicopterData _helicopterData;

        private bool _isEnable;

        public bool IsEnable { set => _isEnable = value; }

        [Inject]
        public void Construct(FinishGameController finishController)
        {
            _helicopterData.FinishController = finishController;
            _helicopterData.TriggerComponent.OnEnter.AddListener(OnTrigger);
        }

        private void OnTrigger(Collider other)
        {
            if (!_isEnable)
                return;
            if (other.TryGetComponent(out PlayerEntity playerEntity))
                _helicopterData.FinishController.EscapeFinish();
        }
    }
}
using App.Content.Player;
using App.Logic;
using UnityEngine;
using VContainer;

namespace App.Content.Helicopter
{
    public sealed class HelicopterEntity : MonoBehaviour
    {
        [SerializeField] private HelicopterData _helicopterData;

        private bool _isInteractable;
        private bool _isEnable;

        public bool IsInteractable { set => _isInteractable = value; }
        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (value == _isEnable)
                    return;
                _isEnable = value;
                if (value)
                    _helicopterData.TriggerComponent.OnEnter.AddListener(OnTrigger);
                else _helicopterData.TriggerComponent.OnEnter.RemoveListener(OnTrigger);
            }
        }


        [Inject]
        public void Construct(FinishGameController finishController)
        {
            _helicopterData.FinishController = finishController;
        }

        private void OnTrigger(Collider other)
        {
            if (!_isInteractable)
                return;
            if (other.TryGetComponent(out PlayerEntity playerEntity))
                _helicopterData.FinishController.EscapeFinish();
        }
    }
}
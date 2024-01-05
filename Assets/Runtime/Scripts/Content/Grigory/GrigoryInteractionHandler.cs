namespace App.Content.Grigory
{
    public sealed class GrigoryInteractionHandler
    {
        private GrigoryData _grigoryData;

        private bool _isEnable;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (value == _isEnable)
                    return;
                _isEnable = value;
                if (value)
                {
                    _grigoryData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
                }

                else
                {
                    _grigoryData.InteractableComp.OnFocusChanged.RemoveListener(OnFocusChanged);
                    _grigoryData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnPerformedInteraction);
                    CloseInteractionIcon();
                }
            }
        }

        public GrigoryInteractionHandler(GrigoryData grigoryData)
            => _grigoryData = grigoryData;

        private void OnFocusChanged(bool obj)
        {
            if (obj)
            {
                ShowInteractionIcon();
                _grigoryData.AppInputSystem.SetInteractionTime(_grigoryData.InteractTime);
                _grigoryData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
            }
            else
            {
                CloseInteractionIcon();
                _grigoryData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnPerformedInteraction);
            }
        }
        private void CloseInteractionIcon()
        {
            if (_grigoryData.InteractIcon == null)
                return;
            _grigoryData.InteractionIconFactory.Remove(_grigoryData.InteractIcon);
        }
        private void ShowInteractionIcon()
        {
            _grigoryData.InteractIcon = _grigoryData.InteractionIconFactory.Create();
            _grigoryData.InteractIcon.SetPosition(_grigoryData.InteractionIconPosition);
            _grigoryData.InteractIcon.IsEnable = true;
            _grigoryData.InteractIcon.OpenTip();
            _grigoryData.InteractIcon.HoldMode = false;
        }
        private void OnPerformedInteraction()
            => _grigoryData.UIController.OpenGrigoryMenu(_grigoryData.StorageInventory);
    }
}
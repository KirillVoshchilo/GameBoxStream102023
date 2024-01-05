namespace App.Content.Fevronia
{
    public sealed class FevroniaInteractionHandler
    {
        private FevroniaData _fevroniaData;

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
                    _fevroniaData.InteractableComp.OnFocusChanged.AddListener(OnFocusChanged);
                }

                else
                {
                    _fevroniaData.InteractableComp.OnFocusChanged.RemoveListener(OnFocusChanged);
                    _fevroniaData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnPerformedInteraction);
                    CloseInteractionIcon();
                }
            }
        }

        public FevroniaInteractionHandler(FevroniaData fevroniaData)
            => _fevroniaData = fevroniaData;

        private void OnFocusChanged(bool obj)
        {
            if (obj)
            {
                ShowInteractionIcon();
                _fevroniaData.AppInputSystem.SetInteractionTime(_fevroniaData.InteractTime);
                _fevroniaData.AppInputSystem.OnInteractionPerformed.AddListener(OnPerformedInteraction);
            }
            else
            {
                CloseInteractionIcon();
                _fevroniaData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnPerformedInteraction);
            }
        }
        private void CloseInteractionIcon()
        {
            if (_fevroniaData.InteractIcon == null)
                return;
            _fevroniaData.InteractionIconFactory.Remove(_fevroniaData.InteractIcon);
        }
        private void ShowInteractionIcon()
        {
            _fevroniaData.InteractIcon = _fevroniaData.InteractionIconFactory.Create();
            _fevroniaData.InteractIcon.SetPosition(_fevroniaData.InteractionIconPosition);
            _fevroniaData.InteractIcon.IsEnable = true;
            _fevroniaData.InteractIcon.OpenTip();
            _fevroniaData.InteractIcon.HoldMode = false;
        }
        private void OnPerformedInteraction()
            => _fevroniaData.UIController.OpenFevroniaMenu();
    }
}
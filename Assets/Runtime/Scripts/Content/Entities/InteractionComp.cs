using App.Simples;
using System;

namespace App.Content.Entities
{
    [Serializable]
    public sealed class InteractionComp
    {
        private bool _isInFocus;
        private readonly SEvent<bool> _onFocusChanged = new();

        public bool IsInFocus
        {
            get => _isInFocus;
            set
            {
                _isInFocus = value;
                _onFocusChanged.Invoke(value);
            }
        }
        public SEvent<bool> OnFocusChanged
            => _onFocusChanged;
    }
}
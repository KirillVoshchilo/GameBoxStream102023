using App.Content.Player;
using App.Simples;
using System;

namespace App.Content
{
    public sealed class Activation
    {
        private readonly Action _activate;
        private readonly Action _deactivate;
        
        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value == _isActive)
                    return;
               
                _isActive = value;
             
                if (value)
                    _activate?.Invoke();
                else  _deactivate?.Invoke();
            }
        }

        public Activation(Action activate, Action deactivate)
        {
            _activate = activate;
            _deactivate = deactivate;
        }
    }
}
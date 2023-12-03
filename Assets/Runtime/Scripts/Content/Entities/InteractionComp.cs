using App.Simples;
using System;
using UnityEngine;

namespace App.Content.Entities
{
    [Serializable]
    public sealed class InteractionComp
    {
        private bool _isInFocus;
        private readonly SEvent<bool> _onFocusChanged = new();
        private Transform _transform;
        private IEntity _entity;

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
        public Transform Transform { get => _transform; set => _transform = value; }
        public IEntity Entity { get => _entity; set => _entity = value; }
    }
}
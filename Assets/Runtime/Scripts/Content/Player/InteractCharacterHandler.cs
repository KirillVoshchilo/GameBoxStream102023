using UnityEngine;

namespace App.Content.Player
{
    public sealed class InteractCharacterHandler
    {
        private PlayerData _playerData;
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
                    _playerData.TriggerComponent.OnExit.AddListener(OnExitEntity);
                    _playerData.TriggerComponent.OnEnter.AddListener(OnEnterEntity);
                }
                else
                {
                    _playerData.TriggerComponent.OnExit.RemoveListener(OnExitEntity);
                    _playerData.TriggerComponent.OnEnter.RemoveListener(OnEnterEntity);
                }
            }
        }

        public InteractCharacterHandler(PlayerData playerData)
        {
            _playerData = playerData;
        }

        private void OnExitEntity(Collider collider)
        {
            if (!collider.TryGetComponent(out IEntity entity))
                return;
            InteractionComp interactableComp = entity.Get<InteractionComp>();
            EntityFlags entityFlags = entity.Get<EntityFlags>();
            if (entityFlags != null && !entityFlags.HasFlag(Flags.NPC))
                return;
            if (interactableComp != null && interactableComp == _playerData.InteractionEntity)
            {
                _playerData.InteractionEntity.IsInFocus = false;
                _playerData.InteractionEntity = null;
            }
        }
        private void OnEnterEntity(Collider collider)
        {
            if (!collider.TryGetComponent(out IEntity entity))
                return;
            InteractionComp interactableComp = entity.Get<InteractionComp>();
            if (!CheckInteractability(interactableComp))
                return;
            if (_playerData.InteractionEntity != null)
                _playerData.InteractionEntity.IsInFocus = false;
            interactableComp.IsInFocus = true;
            interactableComp.OnFocusChanged.AddListener(OnInteractionCompFocusChanged);
            _playerData.InteractionEntity = interactableComp;
        }
        private bool CheckInteractability(InteractionComp interactableComp)
        {
            if (interactableComp == null)
                return false;
            EntityFlags entityFlags = interactableComp.Entity.Get<EntityFlags>();
            if (entityFlags != null && !entityFlags.HasFlag(Flags.NPC))
                return false;
            return true;
        }
        private void OnInteractionCompFocusChanged(bool obj)
        {
            _playerData.InteractionEntity.OnFocusChanged.RemoveListener(OnInteractionCompFocusChanged);
            _playerData.InteractionEntity = null;
        }
    }
}
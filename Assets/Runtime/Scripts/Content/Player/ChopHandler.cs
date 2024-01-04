using App.Architecture.AppData;
using App.Content.Tree;
using App.Simples.CellsInventory;
using App.Simples;
using UnityEngine;

namespace App.Content.Player
{
    public sealed class ChopHandler
    {
        private static readonly int s_isChoping = Animator.StringToHash("IsChoping");

        private readonly PlayerData _playerData;
        private bool _hasTreeInFocus;
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
                    _playerData.PlayerAnimationsEvents.OnAxeHited.AddListener(_playerData.AxeHitSound.Play);
                    _playerData.AppInputSystem.OnInteractionStarted.AddListener(OnInteractionStarted);
                    _playerData.AppInputSystem.OnInteractionCanceled.AddListener(OnInteractionCanceled);
                    _playerData.AppInputSystem.OnInteractionPerformed.AddListener(OnInteractionPerformed);
                    _playerData.TriggerComponent.OnExit.AddListener(OnExitEntity);
                    _playerData.TriggerComponent.OnEnter.AddListener(OnEnterEntity);
                }
                else
                {
                    _playerData.PlayerAnimationsEvents.OnAxeHited.RemoveListener(_playerData.AxeHitSound.Play);
                    _playerData.AppInputSystem.OnInteractionStarted.RemoveListener(OnInteractionStarted);
                    _playerData.AppInputSystem.OnInteractionCanceled.RemoveListener(OnInteractionCanceled);
                    _playerData.AppInputSystem.OnInteractionPerformed.RemoveListener(OnInteractionPerformed);
                    _playerData.TriggerComponent.OnExit.RemoveListener(OnExitEntity);
                    _playerData.TriggerComponent.OnEnter.RemoveListener(OnEnterEntity);
                }
            }
        }
        private bool IsChoping => _playerData.Animator.GetBool(s_isChoping);

        public ChopHandler(PlayerData playerData)
        {
            _playerData = playerData;
        }
        private void OnExitEntity(Collider collider)
        {
            if (!collider.TryGetComponent(out IEntity entity))
                return;
            EntityFlags entityFlags = entity.Get<EntityFlags>();
            if (entityFlags != null && !entityFlags.HasFlag(Flags.TREE))
                return;
            if (_playerData.AppInputSystem.IsInteracting)
            {
                if (!_playerData.AppInputSystem.IsMoving)
                    _playerData.Walker.IsMovingEnable = false;
                if (IsChoping)
                    StopChoping();
                _playerData.AppInputSystem.PlayerMovingIsEnable = true;
                _playerData.AppInputSystem.InteractionIsEnable = true;
            }
            InteractionComp interactableComp = entity.Get<InteractionComp>();
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
            _hasTreeInFocus = true;
            interactableComp.IsInFocus = true;
            interactableComp.OnFocusChanged.AddListener(OnInteractionCompFocusChanged);
            _playerData.InteractionEntity = interactableComp;
        }
        private bool CheckInteractability(InteractionComp interactableComp)
        {
            if (interactableComp == null)
                return false;
            EntityFlags entityFlags = interactableComp.Entity.Get<EntityFlags>();
            if (entityFlags != null && !entityFlags.HasFlag(Flags.TREE))
                return false;
            InteractionRequirementsComp interactionRequirementsComp = interactableComp.Entity.Get<InteractionRequirementsComp>();
            if (!CheckInteractable(interactionRequirementsComp))
                return false;
            if (!_playerData.PlayerInventory.HasEmptyCells)
                return false;
            return true;
        }
        private bool CheckInteractable(InteractionRequirementsComp interactionRequirementsComp)
        {
            foreach (Alternatives alternative in interactionRequirementsComp.Alternatives)
            {
                if (CheckRequirements(alternative))
                    return true;
            }
            return false;
        }
        private bool CheckRequirements(Alternatives alternative)
        {
            foreach (ItemCount item in alternative.Requirements)
            {
                if (_playerData.PlayerInventory.GetCount(item.Key) < item.Count)
                    return false;
            }
            return true;
        }
        private void OnInteractionCompFocusChanged(bool obj)
        {
            if (!_hasTreeInFocus)
                return;
            _hasTreeInFocus = false;
            _playerData.InteractionEntity.OnFocusChanged.RemoveListener(OnInteractionCompFocusChanged);
            _playerData.InteractionEntity = null;
        }
        private void StopChoping()
        {
            if (_playerData.Animator.GetBool(s_isChoping))
                _playerData.Animator.SetBool(s_isChoping, false);
            if (_playerData.CurrentAxeModel != null)
                Object.Destroy(_playerData.CurrentAxeModel);
        }
        private void OnInteractionPerformed()
            => StopChoping();
        private void OnInteractionCanceled()
            => StopChoping();
        private void OnInteractionStarted()
        {
            if (_playerData.InteractionEntity == null)
                return;
            if (_playerData.InteractionEntity.Entity == null)
                return;
            if (_playerData.InteractionEntity.IsBlocked)
                return;
            if (_playerData.InteractionEntity.Entity is not TreeEntity)
                return;
            _playerData.Animator.SetBool(s_isChoping, true);
            ShowAxe();
        }

        #region View
        private void ShowAxe()
        {
            SSOKey currentAxe = DefineCurrentAxe();
            GameObject prefab = _playerData.Configuration.Models[currentAxe];
            GameObject instance = Object.Instantiate(prefab, _playerData.AxeParent);
            _playerData.CurrentAxeModel = instance;
        }
        private SSOKey DefineCurrentAxe()
        {
            Cell[] cells = _playerData.PlayerInventory.Cells;
            int count = cells.Length;
            for (int i = 0; i < count; i++)
            {
                if (cells[i] == null)
                    continue;
                SSOKey category = _playerData.Configuration.EquipmentConfigurations.GetUpperCategory(cells[i].Key);
                if (category == null)
                    continue;
                if (category == _playerData.AxeCategory)
                    return cells[i].Key;
            }
            return null;
        }
        #endregion
    }
}
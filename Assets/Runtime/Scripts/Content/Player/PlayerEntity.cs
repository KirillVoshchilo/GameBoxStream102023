using App.Architecture;
using App.Architecture.AppData;
using App.Architecture.AppInput;
using App.Content.Entities;
using App.Content.Field;
using App.Logic;
using System;
using UnityEngine;
using VContainer;

namespace App.Content.Player
{
    public sealed class PlayerEntity : MonoBehaviour, IEntity
    {
        [SerializeField] private PlayerData _playerData;

        private PlayerMoveHandler _moveHandler;
        private HeatHandler _heatHandler;
        private BonfireBuildHandler _bonfireBuildHandler;
        private PlayerAnimatorHandler _playerAnimatorHandler;
        private EquipmentViewHandler _equipmentViewHandler;

        public bool IsEnable
        {
            get => _playerData.IsEnable;
            set
            {
                _playerData.IsEnable = value;
                if (value)
                {
                    _playerData.TriggerComponent.OnExit.AddListener(OnExitEntity);
                    _playerData.TriggerComponent.OnEnter.AddListener(OnEnterEntity);
                    _moveHandler.IsEnable = true;
                }
                else
                {
                    _playerData.TriggerComponent.OnExit.ClearListeners();
                    _playerData.TriggerComponent.OnEnter.ClearListeners();
                    _moveHandler.IsEnable = true;
                }
            }
        }
        [Inject]
        public void Construct(IAppInputSystem appInputSystem,
            CamerasStorage camerasStorage,
            Configuration configuration,
            BonfireFactory bonfireFactory)
        {
            _playerData.Configuration = configuration;
            _playerData.BonfireFactory = bonfireFactory;
            _heatHandler = new(_playerData);
            _playerData.Walker = new(_playerData.Rigidbody);
            _playerData.AppInputSystem = appInputSystem;
            _bonfireBuildHandler = new(_playerData);
            _playerAnimatorHandler = new(_playerData);
            _playerData.PlayerAnimationsEvents.OnAxeHited.AddListener(_playerData.AxeHitSound.Play);
            _playerData.PlayerAnimationsEvents.OnStep.AddListener(_playerData.StepSound.Play);
            _playerData.MainCameraTransform = camerasStorage.MainCamera.transform;
            _playerData.PlayerInventory = new Inventory(configuration.PlayerInventoryConfigurations, 9);
            _playerData.HeatData.OnHeatChanged.AddListener(OnHeatChanged);
            bonfireFactory.PlayerInventory = _playerData.PlayerInventory;
            _moveHandler = new PlayerMoveHandler(_playerData)
            {
                IsEnable = true
            };
            _equipmentViewHandler = new EquipmentViewHandler(_playerData);
            _playerData.TriggerComponent.OnExit.AddListener(OnExitEntity);
            _playerData.TriggerComponent.OnEnter.AddListener(OnEnterEntity);
            Debug.Log("Сконструировал PlayerEntity.");
        }

        private void OnHeatChanged(float obj)
        {
            if (!_playerData.HasCoughed && obj < _playerData.HeatData.DefaultHeatValue * 0.2)
            {
                _playerData.HasCoughed = true;
                _playerData.CoughSound.Play();
            }
            if (_playerData.HasCoughed && obj > _playerData.HeatData.DefaultHeatValue * 0.2)
            {
                _playerData.HasCoughed = false;
            }
        }

        public T Get<T>() where T : class
        {
            if (typeof(T) == typeof(WalkerData))
                return _playerData.Walker as T;
            if (typeof(T) == typeof(Inventory))
                return _playerData.PlayerInventory as T;
            if (typeof(T) == typeof(HeatData))
                return _playerData.HeatData as T;
            return null;
        }

        private void OnExitEntity(Collider collider)
        {
            if (!collider.TryGetComponent(out IEntity entity))
                return;
            if (_playerData.AppInputSystem.IsInteracting)
            {
                if (!_playerData.AppInputSystem.IsMoving)
                {
                    _moveHandler.StopMove();
                }
                if (_playerAnimatorHandler.IsCHoping)
                    _playerAnimatorHandler.StopChoping();
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
            InteractableIntityEnter(entity);
        }
        private void InteractableIntityEnter(IEntity entity)
        {
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
            if (interactableComp.Entity is ResourceSourceEntity)
            {
                InteractionRequirementsComp interactionRequirementsComp = interactableComp.Entity.Get<InteractionRequirementsComp>();
                if (!CheckInteractable(interactionRequirementsComp))
                    return false;
                if (!_playerData.PlayerInventory.HasEmptyCells)
                    return false;
            }
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
            _playerData.InteractionEntity.OnFocusChanged.RemoveListener(OnInteractionCompFocusChanged);
            _playerData.InteractionEntity = null;
        }

        private void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            if (_playerData.Walker == null)
                return;
            Gizmos.color = new Color(1, 1, 1, 0.3f);
            Gizmos.DrawSphere(_playerData.BonfireTargetPosition.position, _playerData.BuildCheckcolliderSize);
        }
    }
}
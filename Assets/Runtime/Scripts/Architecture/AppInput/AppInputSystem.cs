using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;
using App.Simples;

namespace App.Architecture.AppInput
{
    public sealed class AppInputSystem : UserInput.IPlayerActions, IAppInputSystem
    {
        private readonly UserInput _interactions;
        private readonly SEvent<float> _onInteractionPercantagechanged = new();
        private readonly SEvent _onMovingStarted = new();
        private readonly SEvent _onMovingStoped = new();
        private readonly SEvent _onLookingStarted = new();
        private readonly SEvent _onLookingStoped = new();
        private readonly SEvent _onInteractionStarted = new();
        private readonly SEvent _onInteractionPerformed = new();
        private readonly SEvent _onInteractionCanceled = new();
        private readonly SEvent _onEscapePressed = new();
        private readonly SEvent _onInventoryPressed = new();
        private readonly SEvent _onInventorySelect = new();
        private readonly SEvent _onBonfireBuilded = new();
        private readonly SEvent _onGoNext = new();
        private readonly SEvent<Vector2> _onMovedInInventory = new();
        private Vector2 _moveDirection;
        private Vector2 _lookDirection;
        private bool _isMoving;
        private bool _isLooking;
        private bool _escapeIsEnable = false;
        private bool _inventoryIsEnable = false;
        private bool _playerMovingIsEnable = false;
        private bool _isGoNextEnable = false;
        private bool _inventoryMoveIsEnable = false;
        private float _interactionTime;
        private bool _isInteracting;

        public Vector2 MoveDirection => _moveDirection;
        public bool IsMoving => _isMoving;
        public bool IsInteracting => _isInteracting;
        public float InteractionPercentage => _interactions.Player.Interact.GetTimeoutCompletionPercentage();
        public SEvent OnMovingStarted => _onMovingStarted;
        public SEvent OnMovingStoped => _onMovingStoped;
        public SEvent OnInteractionStarted => _onInteractionStarted;
        public SEvent OnInteractionPerformed => _onInteractionPerformed;
        public SEvent OnInteractionCanceled => _onInteractionCanceled;
        public SEvent OnEscapePressed => _onEscapePressed;
        public SEvent OnInventoryPressed => _onInventoryPressed;
        public SEvent<float> OnInteractionPercantagechanged => _onInteractionPercantagechanged;
        public bool EscapeIsEnable { get => _escapeIsEnable; set => _escapeIsEnable = value; }
        public bool InventoryIsEnable { get => _inventoryIsEnable; set => _inventoryIsEnable = value; }
        public bool PlayerMovingIsEnable
        {
            get => _playerMovingIsEnable;
            set
            {
                _playerMovingIsEnable = value;
                if (!value)
                {
                    _isMoving = false;
                    _onMovingStoped.Invoke();
                }

            }
        }
        public SEvent<Vector2> OnMovedInInventory => _onMovedInInventory;
        public bool InventoryMoveIsEnable { get => _inventoryMoveIsEnable; set => _inventoryMoveIsEnable = value; }
        public SEvent OnInventorySelected => _onInventorySelect;
        public SEvent OnBonfireBuilded => _onBonfireBuilded;
        public SEvent OnGoNext => _onGoNext;
        public bool IsGoNextEnable { get => _isGoNextEnable; set => _isGoNextEnable = value; }

        public AppInputSystem()
        {
            _interactions = new UserInput();
            _interactions.Player.SetCallbacks(this);
            _interactions.Player.Enable();
        }

        public void SetInteractionTime(float duration)
        {
            _interactions.Player.Interact.ApplyParameterOverride(nameof(HoldInteraction.duration), duration);
            _interactionTime = duration;
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            if (!_playerMovingIsEnable)
                return;
            if (_isInteracting)
                return;
            _moveDirection = context.ReadValue<Vector2>();
            if (_moveDirection != Vector2.zero && !_isMoving)
            {
                _isMoving = true;
                _onMovingStarted.Invoke();
            }
            if (_isMoving && _moveDirection == Vector2.zero)
            {
                _isMoving = false;
                _onMovingStoped.Invoke();
            }
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!_playerMovingIsEnable)
                return;
            _onInteractionPercantagechanged.Invoke(_interactions.Player.Interact.GetTimeoutCompletionPercentage());
            if (context.phase == InputActionPhase.Canceled)
            {
                _onInteractionCanceled.Invoke();
                _isInteracting = false;
            }
            if (context.phase == InputActionPhase.Started)
            {
                _onInteractionStarted.Invoke();
                _isInteracting = true;
            }
            if (context.phase == InputActionPhase.Started && _interactionTime == 0)
            {
                _isInteracting = false;
                _onInteractionPerformed.Invoke();
            }
            else if (context.phase == InputActionPhase.Performed)
            {
                _isInteracting = false;
                _onInteractionPerformed.Invoke();
            }
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            _lookDirection = context.ReadValue<Vector2>();
            if (_lookDirection != Vector2.zero && !_isMoving)
            {
                _isLooking = true;
                _onLookingStarted.Invoke();
            }
            if (_isLooking && _lookDirection == Vector2.zero)
            {
                _isLooking = false;
                _onLookingStoped.Invoke();
            }
        }
        public void OnEsc(InputAction.CallbackContext context)
        {
            if (!_escapeIsEnable)
                return;
            if (context.phase == InputActionPhase.Performed)
                _onEscapePressed.Invoke();
        }
        public void OnInventory(InputAction.CallbackContext context)
        {
            if (!_inventoryIsEnable)
                return;
            if (context.phase == InputActionPhase.Performed)
                _onInventoryPressed.Invoke();
        }

        public void OnInventoryMove(InputAction.CallbackContext context)
        {
            if (!_inventoryMoveIsEnable)
                return;
            Vector2 direction = context.ReadValue<Vector2>();
            if (context.phase == InputActionPhase.Performed)
                _onMovedInInventory.Invoke(direction);
        }

        public void OnInventorySelect(InputAction.CallbackContext context)
        {
            if (!_inventoryMoveIsEnable)
                return;
            if (context.phase == InputActionPhase.Performed)
                _onInventorySelect.Invoke();
        }

        public void OnBuildBonfire(InputAction.CallbackContext context)
        {
            if (!_playerMovingIsEnable)
                return;
            if (context.phase == InputActionPhase.Performed)
                _onBonfireBuilded.Invoke();
        }

        public void OnGoNextSlide(InputAction.CallbackContext context)
        {
            if (!_isGoNextEnable)
                return;
            if (context.phase == InputActionPhase.Performed)
                _onGoNext.Invoke();
        }
    }

    public interface IAppInputSystem
    {
        float InteractionPercentage { get; }
        bool IsMoving { get; }
        Vector2 MoveDirection { get; }
        SEvent OnEscapePressed { get; }
        SEvent OnInteractionCanceled { get; }
        SEvent OnInteractionPerformed { get; }
        SEvent OnInteractionStarted { get; }
        SEvent OnInventoryPressed { get; }
        SEvent OnMovingStarted { get; }
        SEvent OnMovingStoped { get; }
        SEvent<float> OnInteractionPercantagechanged { get; }
        bool EscapeIsEnable { get; set; }
        bool InventoryIsEnable { get; set; }
        bool PlayerMovingIsEnable { get; set; }
        bool InventoryMoveIsEnable { get; set; }
        SEvent<Vector2> OnMovedInInventory { get; }
        SEvent OnInventorySelected { get; }
        SEvent OnBonfireBuilded { get; }
        SEvent OnGoNext { get; }
        bool IsGoNextEnable { get; set; }
        bool IsInteracting { get; }

        void SetInteractionTime(float duration);
    }
}
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Content.Player
{
    public sealed class PlayerMoveHandler
    {
        private static readonly int s_isMoving = Animator.StringToHash("IsMoving");
        private static readonly int s_walkBlend = Animator.StringToHash("Blend");

        private readonly PlayerData _playerData;
        private bool _isEnable;
        private float _targetBlendSpeed;
        private float _currentBlendSpeed;

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
                    _playerData.AppInputSystem.OnMovingStarted.AddListener(StartMove);
                    _playerData.AppInputSystem.OnMovingStoped.AddListener(StopMove);
                    _playerData.PlayerAnimationsEvents.OnStep.AddListener(_playerData.StepSound.Play);
                    _playerData.Walker.OnSpeedChanged.AddListener(OnMovingSpeedChanged);
                    _playerData.Walker.OnMovingEnableChanged.AddListener(OnMovingEnableChanged);
                    _playerData.AppInputSystem.OnInteractionStarted.AddListener(RotateToInteraction);
                }
                else
                {
                    _playerData.AppInputSystem.OnMovingStarted.RemoveListener(StartMove);
                    _playerData.AppInputSystem.OnMovingStoped.RemoveListener(StopMove);
                    _playerData.PlayerAnimationsEvents.OnStep.RemoveListener(_playerData.StepSound.Play);
                    _playerData.Walker.OnSpeedChanged.RemoveListener(OnMovingSpeedChanged);
                    _playerData.Walker.OnMovingEnableChanged.RemoveListener(OnMovingEnableChanged);
                    _playerData.AppInputSystem.OnInteractionStarted.RemoveListener(RotateToInteraction);
                    StopMove();
                }
            }
        }

        public PlayerMoveHandler(PlayerData playerData)
        {
            _playerData = playerData;
            _playerData.Walker.IsMoving = false;
            _playerData.Walker.MovingSpeed = _playerData.DefaultMovingSpeed;
        }

        private void OnMovingEnableChanged(bool obj)
        {
            if (obj)
                return;
            StopMove();
        }

        private void RotateToInteraction()
        {
            if (_playerData.InteractionEntity == null)
                return;
            if (_playerData.InteractionEntity.Transform == null)
                return;
            Vector3 target = _playerData.Transform.position;
            target.x = _playerData.InteractionEntity.Transform.position.x;
            target.z = _playerData.InteractionEntity.Transform.position.z;
            _playerData.Transform.LookAt(target);
        }
        private void StartMove()
        {
            _playerData.Walker.IsMoving = true;
            MoveProcess()
                .Forget();
            StartWalkAnimation();
        }
        private void StopMove()
        {
            _playerData.Walker.IsMoving = false;
            StopWalkAnimation();
        }
        private async UniTask MoveProcess()
        {
            while (_playerData.Walker.IsMoving && _isEnable && _playerData.AppInputSystem.IsMoving)
            {
                Move();
                Rotate();
                await UniTask.DelayFrame(1);
            }
            _playerData.Walker.IsMoving = false;
        }
        private void Rotate()
        {
            Vector3 target = _playerData.Transform.position + _playerData.Walker.MovingDirection;
            _playerData.Transform.LookAt(target);
        }
        private float CalculateSpeed()
        {
            float resultSpeed = _playerData.DefaultMovingSpeed;
            foreach (float multiplier in _playerData.Walker.SpeedMultipliers.Values)
                resultSpeed *= multiplier;
            return resultSpeed;
        }
        private void Move()
        {
            Vector3 target = _playerData.Transform.position;
            Vector3 forwarDirection = _playerData.MainCameraTransform.forward - (Vector3.up * Vector3.Dot(_playerData.MainCameraTransform.forward, Vector3.up));
            target += _playerData.AppInputSystem.MoveDirection.x * _playerData.MainCameraTransform.right;
            target += _playerData.AppInputSystem.MoveDirection.y * forwarDirection.normalized;
            _playerData.Walker.MovingDirection = (target - _playerData.Transform.position).normalized;
            _playerData.Walker.MovingSpeed = CalculateSpeed();
            _playerData.Rigidbody.MovePosition((_playerData.Walker.MovingDirection * _playerData.Walker.MovingSpeed) + _playerData.Transform.position);
        }
        #region View
        private void OnMovingSpeedChanged(float obj)
            => _targetBlendSpeed = Mathf.Clamp(obj / _playerData.DefaultMovingSpeed, 0, 1);
        private async UniTask ChangeAnimationSpeedProcess()
        {
            while (_playerData.Walker.IsMoving)
            {
                if (_currentBlendSpeed < _targetBlendSpeed)
                {
                    _currentBlendSpeed = Mathf.Clamp(_currentBlendSpeed + Time.deltaTime, 0, _targetBlendSpeed);
                    _playerData.Animator.SetFloat(s_walkBlend, _currentBlendSpeed);
                }
                if (_currentBlendSpeed > _targetBlendSpeed)
                {
                    _currentBlendSpeed = Mathf.Clamp(_currentBlendSpeed - Time.deltaTime, _targetBlendSpeed, _currentBlendSpeed);
                    _playerData.Animator.SetFloat(s_walkBlend, _currentBlendSpeed);
                }
                await UniTask.NextFrame();
            }
        }
        private void StartWalkAnimation()
        {
            _playerData.Animator.SetBool(s_isMoving, true);
            ChangeAnimationSpeedProcess()
                .Forget();
        }
        private void StopWalkAnimation()
            => _playerData.Animator.SetBool(s_isMoving, false);
        #endregion
    }
}
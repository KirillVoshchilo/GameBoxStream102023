using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace App.Content.Player
{
    public sealed class PlayerMoveHandler
    {
        private readonly PlayerData _playerData;
        private bool _isEnable;

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                if (value)
                {
                    _playerData.AppInputSystem.OnMovingStarted.AddListener(StartMove);
                    _playerData.AppInputSystem.OnMovingStoped.AddListener(StopMove);
                }
                else
                {
                    _playerData.AppInputSystem.OnMovingStarted.RemoveListener(StartMove);
                    _playerData.AppInputSystem.OnMovingStoped.RemoveListener(StopMove);
                    StopMove();
                }
            }
        }

        public PlayerMoveHandler(PlayerData playerBlackboard)
        {
            _playerData = playerBlackboard;
            _playerData.Walker.IsMoving = false;
            _playerData.Walker.MovingSpeed = _playerData.DefaultMovingSpeed;
            _playerData.AppInputSystem.OnInteractionStarted.AddListener(RotateToInteraction);
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
        }
        private void StopMove()
            => _playerData.Walker.IsMoving = false;
        private async UniTask MoveProcess()
        {
            while (_playerData.Walker.IsMoving && _isEnable)
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
            {
                resultSpeed *= multiplier;
                Debug.Log($"multiplier {multiplier} resultSpeed {resultSpeed}");
            }
            Debug.Log($"DefaultMovingSpeed {_playerData.DefaultMovingSpeed} resultSpeed {resultSpeed}");
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
    }
}
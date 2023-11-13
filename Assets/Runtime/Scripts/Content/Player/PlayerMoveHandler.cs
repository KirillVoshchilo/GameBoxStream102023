using Cysharp.Threading.Tasks;
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
            _playerData.IsMoving = false;
            _playerData.MovingSpeed = _playerData.DefaultMovingSpeed;
        }

        private void StartMove()
        {
            _playerData.IsMoving = true;
            MoveProcess()
                .Forget();
        }
        private void StopMove()
            => _playerData.IsMoving = false;
        private async UniTask MoveProcess()
        {
            while (_playerData.IsMoving && _isEnable)
            {
                Move();
                await UniTask.DelayFrame(1);
            }
            _playerData.IsMoving = false;
        }
        private void Move()
        {
            Vector3 target = _playerData.Transform.position;
            Vector3 forwarDirection = _playerData.MainCameraTransform.forward - (Vector3.up * Vector3.Dot(_playerData.MainCameraTransform.forward, Vector3.up));
            target += _playerData.AppInputSystem.MoveDirection.x * _playerData.MovingSpeed * _playerData.MainCameraTransform.right;
            target += _playerData.AppInputSystem.MoveDirection.y * _playerData.MovingSpeed * forwarDirection.normalized;
            _playerData.Rigidbody.MovePosition(target);
        }
    }
}
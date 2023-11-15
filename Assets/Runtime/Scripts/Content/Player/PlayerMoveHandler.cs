using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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
                await UniTask.DelayFrame(1);
            }
            _playerData.Walker.IsMoving = false;
        }
        private float CalcSpeed(Vector3 moveDirection)
        {
            float resultSpeed = _playerData.Walker.MovingSpeed;
            Vector3 nextPoint = _playerData.Transform.position + moveDirection * _playerData.SearchingGroundDistance;
            Ray ray = new(nextPoint, Vector3.down);
            if (!Physics.Raycast(ray, out RaycastHit hit))
                return resultSpeed;
            if (!hit.collider.gameObject.TryGetComponent(out Snow snow))
                return resultSpeed;
            float height = snow.SnowHeightData.GetHeightByCoordinates(hit.textureCoord);
            Debug.Log($"height= {height}");
            if (height > 0)
                resultSpeed *= 0.4f;
            return resultSpeed;
        }
        private void Move()
        {
            Vector3 target = _playerData.Transform.position;
            Vector3 forwarDirection = _playerData.MainCameraTransform.forward - (Vector3.up * Vector3.Dot(_playerData.MainCameraTransform.forward, Vector3.up));
            target += _playerData.AppInputSystem.MoveDirection.x * _playerData.MainCameraTransform.right;
            target += _playerData.AppInputSystem.MoveDirection.y * forwarDirection.normalized;
            Vector3 moveDirection = (target - _playerData.Transform.position).normalized;
            float speed = CalcSpeed(moveDirection);
            Debug.Log($"Speed= {speed}");
            _playerData.Rigidbody.MovePosition((moveDirection * speed) + _playerData.Transform.position);
        }
    }
}
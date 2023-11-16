using App.Content.Entities;
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
            if (!CheckForSnow(nextPoint, out RaycastHit hit, out VirtualHeightMap heightMap))
                return resultSpeed;
            float height = heightMap.GetHeightByCoordinates(hit.textureCoord);
            if (height > 0)
                resultSpeed *= 0.4f;
            return resultSpeed;
        }
        private bool CheckForSnow(Vector3 point, out RaycastHit hit, out VirtualHeightMap heightMap)
        {
            Ray ray = new(point, Vector3.down);
            RaycastHit[] raycasts = Physics.RaycastAll(ray);
            foreach (RaycastHit raycast in raycasts)
            {
                if (raycast.collider.gameObject.TryGetComponent(out IEntity entity))
                {
                    heightMap = entity.Get<VirtualHeightMap>();
                    if (heightMap != null)
                    {
                        hit = raycast;
                        return true;
                    }
                }
            }
            hit = default;
            heightMap = null;
            return false;
        }
        private void Move()
        {
            Vector3 target = _playerData.Transform.position;
            Vector3 forwarDirection = _playerData.MainCameraTransform.forward - (Vector3.up * Vector3.Dot(_playerData.MainCameraTransform.forward, Vector3.up));
            target += _playerData.AppInputSystem.MoveDirection.x * _playerData.MainCameraTransform.right;
            target += _playerData.AppInputSystem.MoveDirection.y * forwarDirection.normalized;
            Vector3 moveDirection = (target - _playerData.Transform.position).normalized;
            float speed = CalcSpeed(moveDirection);
            _playerData.Rigidbody.MovePosition((moveDirection * speed) + _playerData.Transform.position);
        }
    }
}
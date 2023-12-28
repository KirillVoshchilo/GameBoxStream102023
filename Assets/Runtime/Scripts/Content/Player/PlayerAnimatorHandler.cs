using App.Content.Tree;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Content.Player
{
    public sealed class PlayerAnimatorHandler
    {
        private static readonly int s_walkBlend = Animator.StringToHash("Blend");
        private static readonly int s_isMoving = Animator.StringToHash("IsMoving");
        private static readonly int s_isChoping = Animator.StringToHash("IsChoping");
        private static readonly int s_buildBonfireTrigger = Animator.StringToHash("BuildBonfire");

        private readonly PlayerData _playerData;
        private readonly WalkerData _playerWalkerData;
        private float _targetBlendSpeed;
        private float _currentBlendSpeed;

        public bool IsChoping => _playerData.Animator.GetBool(s_isChoping);

        public PlayerAnimatorHandler(PlayerData playerData)
        {
            _playerData = playerData;
            _playerWalkerData = playerData.Walker;
            playerData.Walker.OnMovingStarted.AddListener(OnMovingStarted);
            playerData.Walker.OnMovingStopped.AddListener(OnMovingEnded);
            playerData.Walker.OnSpeedChanged.AddListener(OnMovingSpeedChanged);
            playerData.AppInputSystem.OnInteractionStarted.AddListener(OnInteractionStarted);
            playerData.AppInputSystem.OnInteractionCanceled.AddListener(OnInteractionCanceled);
            playerData.AppInputSystem.OnInteractionPerformed.AddListener(OnInteractionPerformed);
            playerData.AppInputSystem.OnBonfireBuilded.AddListener(OnBonfireBuildStarted);
            _playerData.PlayerAnimationsEvents.OnBonfireBuilded.AddListener(OnBonfireBuilded);
        }

        public void StopChoping()
        {
            if (_playerData.Animator.GetBool(s_isChoping))
                _playerData.Animator.SetBool(s_isChoping, false);
        }

        private void OnBonfireBuilded()
        {
            _playerData.AppInputSystem.InteractionIsEnable = true;
            _playerData.AppInputSystem.PlayerMovingIsEnable = true;
            _playerData.AppInputSystem.InventoryIsEnable = true;
        }
        private void OnBonfireBuildStarted()
        {
            if (!_playerData.CanBuildBonfire)
                return;
            _playerData.Animator.SetTrigger(s_buildBonfireTrigger);
            _playerData.AppInputSystem.PlayerMovingIsEnable = false;
            _playerData.AppInputSystem.InventoryIsEnable = false;
            _playerData.AppInputSystem.InteractionIsEnable = false;
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
            StartChoping();
        }
        private void StartChoping()
        {
            if (_playerData.InteractionEntity.Entity is not TreeEntity)
                return;
            _playerData.Animator.SetBool(s_isChoping, true);
        }
        private void OnMovingSpeedChanged(float obj)
            => _targetBlendSpeed = Mathf.Clamp(obj / _playerData.DefaultMovingSpeed, 0, 1);
        private async UniTask ChangeSpeedProcess()
        {
            while (_playerWalkerData.IsMoving)
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
        private void OnMovingStarted()
        {
            _playerData.Animator.SetBool(s_isMoving, true);
            ChangeSpeedProcess()
                .Forget();
        }
        private void OnMovingEnded() 
            => _playerData.Animator.SetBool(s_isMoving, false);
    }
}
using UnityEngine;

namespace App.Content.Player
{
    public sealed class BonfireBuildHandler
    {
        private static readonly int s_buildBonfireTrigger = Animator.StringToHash("BuildBonfire");

        private readonly PlayerData _playerData;
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
                    _playerData.PlayerAnimationsEvents.OnBonfireSetted.AddListener(OnBonfireSetted);
                    _playerData.AppInputSystem.OnBonfireBuilded.AddListener(OnBonfireBuildStarted);
                    _playerData.PlayerAnimationsEvents.OnBonfireBuilded.AddListener(OnBonfireBuilded);
                }
                else
                {
                    _playerData.PlayerAnimationsEvents.OnBonfireSetted.RemoveListener(OnBonfireSetted);
                    _playerData.AppInputSystem.OnBonfireBuilded.RemoveListener(OnBonfireBuildStarted);
                    _playerData.PlayerAnimationsEvents.OnBonfireBuilded.RemoveListener(OnBonfireBuilded);
                }
            }
        }

        public BonfireBuildHandler(PlayerData playerData)
        {
            _playerData = playerData;
        }

        private void OnBonfireSetted()
            => _playerData.BonfireFactory.Create();
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
    }
}
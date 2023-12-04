using App.Content.Field;
using App.Content.Player;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PlayerAnimatorHandler
{
    private static readonly int WALK_BLEND = Animator.StringToHash("Blend");
    private static readonly int IS_MOVING = Animator.StringToHash("IsMoving");
    private static readonly int IS_CHOPING = Animator.StringToHash("IsChoping");
    private static readonly int BUILD_BONFIRE_TRIGGER = Animator.StringToHash("BuildBonfire");

    private readonly PlayerData _playerData;
    private readonly WalkerData _playerWalkerData;
    private float _targetBlendSpeed;
    private float _currentBlendSpeed;

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
        _playerData.Animator.SetTrigger(BUILD_BONFIRE_TRIGGER);
        _playerData.AppInputSystem.PlayerMovingIsEnable = false;
        _playerData.AppInputSystem.InventoryIsEnable = false;
        _playerData.AppInputSystem.InteractionIsEnable = false;
    }
    private void OnInteractionPerformed()
    {
        StopChoping();
    }
    private void OnInteractionCanceled()
    {
        StopChoping();
    }
    private void OnInteractionStarted()
    {
        if (_playerData.InteractionEntity == null)
            return;
        if (_playerData.InteractionEntity.Entity == null)
            return;
        StartChoping();
    }
    private void StopChoping()
    {
        if (_playerData.Animator.GetBool(IS_CHOPING))
            _playerData.Animator.SetBool(IS_CHOPING, false);
    }
    private void StartChoping()
    {
        if (_playerData.InteractionEntity.Entity is not ResourceSourceEntity)
            return;
        _playerData.Animator.SetBool(IS_CHOPING, true);
    }

    private void OnMovingSpeedChanged(float obj)
    {
        _targetBlendSpeed = Mathf.Clamp(obj / _playerData.DefaultMovingSpeed, 0, 1);
    }
    private async UniTask ChangeSpeedProcess()
    {
        while (_playerWalkerData.IsMoving)
        {
            if (_currentBlendSpeed < _targetBlendSpeed)
            {
                _currentBlendSpeed = Mathf.Clamp(_currentBlendSpeed + Time.deltaTime, 0, _targetBlendSpeed);
                _playerData.Animator.SetFloat(WALK_BLEND, _currentBlendSpeed);
            }
            if (_currentBlendSpeed > _targetBlendSpeed)
            {
                _currentBlendSpeed = Mathf.Clamp(_currentBlendSpeed - Time.deltaTime, _targetBlendSpeed, _currentBlendSpeed);
                _playerData.Animator.SetFloat(WALK_BLEND, _currentBlendSpeed);
            }
            await UniTask.NextFrame();
        }
    }

    private void OnMovingStarted()
    {
        _playerData.Animator.SetBool(IS_MOVING, true);
        ChangeSpeedProcess()
            .Forget();
    }
    private void OnMovingEnded()
    {
        _playerData.Animator.SetBool(IS_MOVING, false);
    }

}

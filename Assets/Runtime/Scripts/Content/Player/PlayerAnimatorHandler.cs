using App.Content.Player;
using UnityEngine;

public class PlayerAnimatorHandler
{
    private static readonly int WALK_BLEND = Animator.StringToHash("Blend");
    private static readonly int IS_MOVING = Animator.StringToHash("IsMoving");

    private readonly PlayerData _playerData;
    private readonly WalkerData _playerWalkerData;

    public PlayerAnimatorHandler(PlayerData playerData)
    {
        _playerData = playerData;
        _playerWalkerData = playerData.Walker;
        playerData.Walker.OnMovingStarted.AddListener(OnMovingStarted);
        playerData.Walker.OnMovingStopped.AddListener(OnMovingEnded);
        playerData.Walker.OnSpeedChanged.AddListener(OnMovingSpeedChanged);
    }

    private void OnMovingSpeedChanged(float obj)
    {
        float animationSpeed = Mathf.Clamp(obj / _playerData.DefaultMovingSpeed, 0, 1);
        _playerData.Animator.SetFloat(WALK_BLEND, animationSpeed);
    }

    private void OnMovingStarted()
    {
        _playerData.Animator.SetBool(IS_MOVING, true);
    }
    private void OnMovingEnded()
    {
        _playerData.Animator.SetBool(IS_MOVING, false);
    }

}

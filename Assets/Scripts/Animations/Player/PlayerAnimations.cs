using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator playerAnimator;
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        switch (Player.Instance.GetPlayerState())
        {
            case Player.PlayerMovementState.Idle:
                playerAnimator.SetBool("Walk", false);
                playerAnimator.SetBool("IdleRun", false);
                playerAnimator.SetBool("Run", false);
                playerAnimator.SetBool("IdleCrouch", false);
                playerAnimator.SetBool("Crouch", false);
                playerAnimator.SetBool("Jump", false);
                playerAnimator.SetBool("Idle", true);
            break;
            case Player.PlayerMovementState.Walk:
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("IdleRun", false);
                playerAnimator.SetBool("Run", false);
                playerAnimator.SetBool("IdleCrouch", false);
                playerAnimator.SetBool("Crouch", false);
                playerAnimator.SetBool("Jump", false);
                playerAnimator.SetBool("Walk", true);
            break;
            case Player.PlayerMovementState.IdleRun:
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("Walk", false);
                playerAnimator.SetBool("Run", false);
                playerAnimator.SetBool("IdleCrouch", false);
                playerAnimator.SetBool("Crouch", false);
                playerAnimator.SetBool("Jump", false);
                playerAnimator.SetBool("IdleRun", true);
            break;
            case Player.PlayerMovementState.Run:
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("Walk", false);
                playerAnimator.SetBool("IdleRun", false);
                playerAnimator.SetBool("IdleCrouch", false);
                playerAnimator.SetBool("Crouch", false);
                playerAnimator.SetBool("Jump", false);
                playerAnimator.SetBool("Run", true);
            break;
            case Player.PlayerMovementState.IdleCrouch:
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("Walk", false);
                playerAnimator.SetBool("IdleRun", false);
                playerAnimator.SetBool("Run", false);
                playerAnimator.SetBool("Crouch", false);
                playerAnimator.SetBool("Jump", false);
                playerAnimator.SetBool("IdleCrouch", true);
            break;
            case Player.PlayerMovementState.Crouch:
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("Walk", false);
                playerAnimator.SetBool("IdleRun", false);
                playerAnimator.SetBool("Run", false);
                playerAnimator.SetBool("IdleCrouch", false);
                playerAnimator.SetBool("Jump", false);
                playerAnimator.SetBool("Crouch", true);
            break;
            case Player.PlayerMovementState.Jump:
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("Walk", false);
                playerAnimator.SetBool("IdleRun", false);
                playerAnimator.SetBool("Run", false);
                playerAnimator.SetBool("IdleCrouch", false);
                playerAnimator.SetBool("Crouch", false);
                playerAnimator.SetBool("Jump", true);
            break;
        }
    }
}

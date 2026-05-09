using System;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    bool onAir = false;
    bool jumpAnimTransitionFixed = false;
    bool jumped = false;
    Player.PlayerMovementState savedPlayerState;
    void Start()
    {
        InputManager.Instance.OnRunPerformed += InputManager_Run;
        InputManager.Instance.OnRunCanceled += InputManager_StopRun;
        InputManager.Instance.OnCrouchPerformed += InputManager_Crouch;
        InputManager.Instance.OnCrouchCanceled += InputManager_StopCrouch;
        InputManager.Instance.OnJumpPerformed += InputManager_Jump;
        Player.Instance.OnLanded += Player_OnLanded;
    }
    void Update()
    {
        if (onAir)
        {
            Player.Instance.SetPlayerState(Player.PlayerMovementState.Jump);
            return;
        }
        else if (!jumpAnimTransitionFixed)
        {
            Player.Instance.SetPlayerState(savedPlayerState);
            jumpAnimTransitionFixed = true;
            jumped = false;
        }
        Vector2 inputVector = InputManager.Instance.GetWalkInputVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            switch (Player.Instance.GetPlayerState())
            {
                case Player.PlayerMovementState.Idle:
                    Player.Instance.SetPlayerState(Player.PlayerMovementState.Walk);
                break;
                case Player.PlayerMovementState.IdleRun:
                    Player.Instance.SetPlayerState(Player.PlayerMovementState.Run);
                break;
                case Player.PlayerMovementState.IdleCrouch:
                    Player.Instance.SetPlayerState(Player.PlayerMovementState.Crouch);
                break;
            }
        }
        else
        {
            switch (Player.Instance.GetPlayerState())
            {
                case Player.PlayerMovementState.Walk:
                    Player.Instance.SetPlayerState(Player.PlayerMovementState.Idle);
                break;
                case Player.PlayerMovementState.Run:
                    Player.Instance.SetPlayerState(Player.PlayerMovementState.IdleRun);
                break;
                case Player.PlayerMovementState.Crouch:
                    Player.Instance.SetPlayerState(Player.PlayerMovementState.IdleCrouch);
                break;
            }
        }
    }
    void InputManager_Run(object sender, EventArgs e)
    {
        Player.Instance.SetPlayerState(Player.PlayerMovementState.Run);
        savedPlayerState = Player.Instance.GetPlayerState();
    }
    void InputManager_StopRun(object sender, EventArgs e)
    {
        Player.Instance.SetPlayerState(Player.PlayerMovementState.Walk);
        savedPlayerState = Player.Instance.GetPlayerState();
    }
    void InputManager_Crouch(object sender, EventArgs e)
    {
        Player.Instance.SetPlayerState(Player.PlayerMovementState.Crouch);
        savedPlayerState = Player.Instance.GetPlayerState();
    }
    void InputManager_StopCrouch(object sender, EventArgs e)
    {
        Player.Instance.SetPlayerState(Player.PlayerMovementState.Walk);
        savedPlayerState = Player.Instance.GetPlayerState();
    }
    void InputManager_Jump(object sender, EventArgs e)
    {
        if (!jumped) savedPlayerState = Player.Instance.GetPlayerState();
        jumped = true;
        Player.Instance.SetPlayerState(Player.PlayerMovementState.Jump);
        jumpAnimTransitionFixed = false;
        onAir = true;
    }
    void Player_OnLanded(object sender, EventArgs e)
    {
        onAir = false;
    }
    void OnDestroy()
    {
        InputManager.Instance.OnRunPerformed -= InputManager_Run;
        InputManager.Instance.OnRunCanceled -= InputManager_StopRun;
        InputManager.Instance.OnCrouchPerformed -= InputManager_Crouch;
        InputManager.Instance.OnCrouchCanceled -= InputManager_StopCrouch;
        InputManager.Instance.OnJumpPerformed -= InputManager_Jump;
        Player.Instance.OnLanded -= Player_OnLanded;
    }
}

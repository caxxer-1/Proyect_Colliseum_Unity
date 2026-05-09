using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get; private set;}
    public event EventHandler OnJumpPerformed;
    public event EventHandler OnJumpCanceled;
    public event EventHandler OnRunPerformed;
    public event EventHandler OnRunCanceled;
    public event EventHandler OnCrouchPerformed;
    public event EventHandler OnCrouchCanceled;
    public event EventHandler OnAttackPerformed;
    public event EventHandler OnDashPerformed;
    public event EventHandler OnCameraDragPerformed;
    public event EventHandler OnCameraDragCanceled;
    public event EventHandler OnTogglePausePerformed;
    public event EventHandler OnChangeFOVPerformed;
    PlayerInputActions playerInputActions;
    public enum Actions
    {
        Forward,
        Backward,
        Left,
        Right,
        Jump,
        Crouch,
        Run,
        Dash,
        Attack,
        TogglePause
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Movement.Jump.performed += Jump_Performed;
        playerInputActions.Movement.Jump.canceled += Jump_Canceled;
        playerInputActions.Movement.Run.performed += Run_Performed;
        playerInputActions.Movement.Run.canceled += Run_Canceled;
        playerInputActions.Movement.Crouch.performed += Crouch_Performed;
        playerInputActions.Movement.Crouch.canceled += Crouch_Canceled;
        playerInputActions.Actions.Attack.performed += Attack_Performed;
        playerInputActions.Movement.Dash.performed += Dash_Performed;
        playerInputActions.Camera.Drag.performed += CameraDrag_Performed;
        playerInputActions.Camera.Drag.canceled += CameraDrag_Canceled;
        playerInputActions.Actions.TogglePauseMenu.performed += TogglePause_Performed;
        playerInputActions.Camera.ChangeFOV.performed += ChangeFOV_Performed;
        if (PlayerPrefs.HasKey("KeyBinds"))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("KeyBinds"));
        }
        playerInputActions.Enable();
    }

    public Vector2 GetWalkInputVectorNormalized()
    {
        return playerInputActions.Movement.Walk.ReadValue<Vector2>().normalized;
    }
    void Jump_Performed(InputAction.CallbackContext callback)
    {
        OnJumpPerformed?.Invoke(this, EventArgs.Empty);
    }
    void Jump_Canceled(InputAction.CallbackContext callback)
    {
        OnJumpCanceled?.Invoke(this, EventArgs.Empty);
    }
    void Run_Performed(InputAction.CallbackContext callback)
    {
        OnRunPerformed?.Invoke(this, EventArgs.Empty);
    }
    void Run_Canceled(InputAction.CallbackContext callback)
    {
        OnRunCanceled?.Invoke(this, EventArgs.Empty);
    }
    void Crouch_Performed(InputAction.CallbackContext callback)
    {
        OnCrouchPerformed?.Invoke(this, EventArgs.Empty);
    }
    void Crouch_Canceled(InputAction.CallbackContext callback)
    {
        OnCrouchCanceled?.Invoke(this, EventArgs.Empty);
    }
    void Attack_Performed(InputAction.CallbackContext callback)
    {
        OnAttackPerformed?.Invoke(this, EventArgs.Empty);
    }
    void Dash_Performed(InputAction.CallbackContext callback)
    {
        OnDashPerformed?.Invoke(this, EventArgs.Empty);
    }
    void CameraDrag_Performed(InputAction.CallbackContext callback)
    {
        OnCameraDragPerformed?.Invoke(this, EventArgs.Empty);
    }
    void CameraDrag_Canceled(InputAction.CallbackContext callback)
    {
        OnCameraDragCanceled?.Invoke(this, EventArgs.Empty);
    }
    void TogglePause_Performed(InputAction.CallbackContext callback)
    {
        OnTogglePausePerformed?.Invoke(this, EventArgs.Empty);
    }
    void ChangeFOV_Performed(InputAction.CallbackContext callback)
    {
        OnChangeFOVPerformed?.Invoke(this, EventArgs.Empty);
    }
    public void RebindKeys(Actions actionForRebinding, Action rebindFinished, Action showRebindError)
    {
        InputAction inputAction;
        int bindIndex;
        playerInputActions.Movement.Disable();
        playerInputActions.Actions.Disable();
        switch (actionForRebinding)
        {
            case Actions.Forward:
                inputAction = playerInputActions.Movement.Walk;
                bindIndex = 1;
            break;
            case Actions.Backward:
                inputAction = playerInputActions.Movement.Walk;
                bindIndex = 2;
            break;
            case Actions.Left:
                inputAction = playerInputActions.Movement.Walk;
                bindIndex = 3;
            break;
            case Actions.Right:
                inputAction = playerInputActions.Movement.Walk;
                bindIndex = 4;
            break;
            case Actions.Jump:
                inputAction = playerInputActions.Movement.Jump;
                bindIndex = 0;
            break;
            case Actions.Crouch:
                inputAction = playerInputActions.Movement.Crouch;
                bindIndex = 0;
            break;
            case Actions.Run:
                inputAction = playerInputActions.Movement.Run;
                bindIndex = 0;
            break;
            case Actions.Dash:
                inputAction = playerInputActions.Movement.Dash;
                bindIndex = 0;
            break;
            case Actions.Attack:
                inputAction = playerInputActions.Actions.Attack;
                bindIndex = 0;
            break;
            case Actions.TogglePause:
                inputAction = playerInputActions.Actions.TogglePauseMenu;
                bindIndex = 0;
            break;
            default:
            return;
        }
        InputBinding previousBind = inputAction.bindings[bindIndex];
        bool exitDuplicatedBindingComprobation = false;
        InputActionRebindingExtensions.PerformInteractiveRebinding(inputAction, bindIndex).OnComplete((callback) => {
            foreach (InputAction bindAction in playerInputActions)
            {
                for (int i = 0; i < bindAction.bindings.Count; i++)
                {
                    if (bindAction.bindings[i].isComposite) continue;
                    if (inputAction.bindings[bindIndex] == bindAction.bindings[i] && i == bindIndex) continue;
                    if (inputAction.bindings[bindIndex].effectivePath == bindAction.bindings[i].effectivePath)
                    {
                        inputAction.ApplyBindingOverride(bindIndex, previousBind);
                        showRebindError();
                        exitDuplicatedBindingComprobation = true;
                        break;
                    }
                }
                if (exitDuplicatedBindingComprobation) break;
            }
            playerInputActions.Movement.Enable();
            playerInputActions.Actions.Enable();
            PlayerPrefs.SetString("KeyBinds", playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            rebindFinished();
        }).Start();
    }

    public string UpdateKeyButtonText(Actions action)
    {
        switch (action)
        {
            case Actions.Forward:
            return playerInputActions.Movement.Walk.bindings[1].ToDisplayString();
            case Actions.Backward:
            return playerInputActions.Movement.Walk.bindings[2].ToDisplayString();
            case Actions.Left:
            return playerInputActions.Movement.Walk.bindings[3].ToDisplayString();
            case Actions.Right:
            return playerInputActions.Movement.Walk.bindings[4].ToDisplayString();
            case Actions.Jump:
            return playerInputActions.Movement.Jump.bindings[0].ToDisplayString();
            case Actions.Crouch:
            return playerInputActions.Movement.Crouch.bindings[0].ToDisplayString();
            case Actions.Run:
            return playerInputActions.Movement.Run.bindings[0].ToDisplayString();
            case Actions.Dash:
            return playerInputActions.Movement.Dash.bindings[0].ToDisplayString();
            case Actions.Attack:
            return playerInputActions.Actions.Attack.bindings[0].ToDisplayString();
            case Actions.TogglePause:
            return playerInputActions.Actions.TogglePauseMenu.bindings[0].ToDisplayString();
            default: return "";
        }
    }

    void OnDestroy()
    {
        playerInputActions.Movement.Jump.performed -= Jump_Performed;
        playerInputActions.Movement.Jump.canceled -= Jump_Canceled;
        playerInputActions.Movement.Run.performed -= Run_Performed;
        playerInputActions.Movement.Run.canceled -= Run_Canceled;
        playerInputActions.Movement.Crouch.performed -= Crouch_Performed;
        playerInputActions.Movement.Crouch.canceled -= Crouch_Canceled;
        playerInputActions.Actions.Attack.performed -= Attack_Performed;
        playerInputActions.Movement.Dash.performed -= Dash_Performed;
        playerInputActions.Camera.Drag.performed -= CameraDrag_Performed;
        playerInputActions.Camera.Drag.canceled -= CameraDrag_Canceled;
        playerInputActions.Actions.TogglePauseMenu.performed -= TogglePause_Performed;
        playerInputActions.Camera.ChangeFOV.performed -= ChangeFOV_Performed;
        playerInputActions.Disable();
    }
}
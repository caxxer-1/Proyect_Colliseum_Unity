using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyRebindUI : MonoBehaviour
{
    public event EventHandler OnReturnedToPauseMenu;
    public event EventHandler<bool> OnRebindStateChanged;
    [SerializeField] GameObject invalidRebindingInfo;
    [SerializeField] Button pauseMenuButton;
    [SerializeField] Button forwardKeyButton;
    [SerializeField] Button backwardKeyButton;
    [SerializeField] Button leftKeyButton;
    [SerializeField] Button rightKeyButton;
    [SerializeField] Button jumpKeyButton;
    [SerializeField] Button crouchKeyButton;
    [SerializeField] Button runKeyButton;
    [SerializeField] Button dashKeyButton;
    [SerializeField] Button attackKeyButton;
    [SerializeField] Button togglePauseKeyButton;
    [SerializeField] TMP_Text forwardKeyButtonText;
    [SerializeField] TMP_Text backwardKeyButtonText;
    [SerializeField] TMP_Text leftKeyButtonText;
    [SerializeField] TMP_Text rightKeyButtonText;
    [SerializeField] TMP_Text jumpKeyButtonText;
    [SerializeField] TMP_Text crouchKeyButtonText;
    [SerializeField] TMP_Text runKeyButtonText;
    [SerializeField] TMP_Text dashKeyButtonText;
    [SerializeField] TMP_Text attackKeyButtonText;
    [SerializeField] TMP_Text togglePauseKeyButtonText;
    void Start()
    {
        forwardKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(InputManager.Actions.Forward);
        backwardKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(InputManager.Actions.Backward);
        leftKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(InputManager.Actions.Left);
        rightKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(InputManager.Actions.Right);
        jumpKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(InputManager.Actions.Jump);
        crouchKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(InputManager.Actions.Crouch);
        runKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(InputManager.Actions.Run);
        dashKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(InputManager.Actions.Dash);
        attackKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(InputManager.Actions.Attack);
        togglePauseKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(InputManager.Actions.TogglePause);
        pauseMenuButton.onClick.AddListener(() =>
        {
            OnReturnedToPauseMenu?.Invoke(this, EventArgs.Empty);
        });
        forwardKeyButton.onClick.AddListener(() =>
        {
            OnRebindStateChanged?.Invoke(this, true);
            InputManager.Instance.RebindKeys(InputManager.Actions.Forward, () => FinishRebind(InputManager.Actions.Forward), () => StartCoroutine(ShowInvalidRebindInfo()));
        });
        backwardKeyButton.onClick.AddListener(() =>
        {
            OnRebindStateChanged?.Invoke(this, true);
            InputManager.Instance.RebindKeys(InputManager.Actions.Backward, () => FinishRebind(InputManager.Actions.Backward), () => StartCoroutine(ShowInvalidRebindInfo()));
        });
        leftKeyButton.onClick.AddListener(() =>
        {
            OnRebindStateChanged?.Invoke(this, true);
            InputManager.Instance.RebindKeys(InputManager.Actions.Left, () => FinishRebind(InputManager.Actions.Left), () => StartCoroutine(ShowInvalidRebindInfo()));
        });
        rightKeyButton.onClick.AddListener(() =>
        {
            OnRebindStateChanged?.Invoke(this, true);
            InputManager.Instance.RebindKeys(InputManager.Actions.Right, () => FinishRebind(InputManager.Actions.Right), () => StartCoroutine(ShowInvalidRebindInfo()));
        });
        jumpKeyButton.onClick.AddListener(() =>
        {
            OnRebindStateChanged?.Invoke(this, true);
            InputManager.Instance.RebindKeys(InputManager.Actions.Jump, () => FinishRebind(InputManager.Actions.Jump), () => StartCoroutine(ShowInvalidRebindInfo()));
        });
        crouchKeyButton.onClick.AddListener(() =>
        {
            OnRebindStateChanged?.Invoke(this, true);
            InputManager.Instance.RebindKeys(InputManager.Actions.Crouch, () => FinishRebind(InputManager.Actions.Crouch), () => StartCoroutine(ShowInvalidRebindInfo()));
        });
        runKeyButton.onClick.AddListener(() =>
        {
            OnRebindStateChanged?.Invoke(this, true);
            InputManager.Instance.RebindKeys(InputManager.Actions.Run, () => FinishRebind(InputManager.Actions.Run), () => StartCoroutine(ShowInvalidRebindInfo()));
        });
        dashKeyButton.onClick.AddListener(() =>
        {
            OnRebindStateChanged?.Invoke(this, true);
            InputManager.Instance.RebindKeys(InputManager.Actions.Dash, () => FinishRebind(InputManager.Actions.Dash), () => StartCoroutine(ShowInvalidRebindInfo()));
        });
        attackKeyButton.onClick.AddListener(() =>
        {
            OnRebindStateChanged?.Invoke(this, true);
            InputManager.Instance.RebindKeys(InputManager.Actions.Attack, () => FinishRebind(InputManager.Actions.Attack), () => StartCoroutine(ShowInvalidRebindInfo()));
        });
        togglePauseKeyButton.onClick.AddListener(() =>
        {
            OnRebindStateChanged?.Invoke(this, true);
            InputManager.Instance.RebindKeys(InputManager.Actions.TogglePause, () => FinishRebind(InputManager.Actions.TogglePause), () => StartCoroutine(ShowInvalidRebindInfo()));
        });
    }

    IEnumerator ShowInvalidRebindInfo()
    {
        invalidRebindingInfo.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        invalidRebindingInfo.SetActive(false);
    }
    void FinishRebind(InputManager.Actions action)
    {
        OnRebindStateChanged?.Invoke(this, false);
        switch (action)
        {
            case InputManager.Actions.Forward:
                forwardKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(action);
            break;
            case InputManager.Actions.Backward:
                backwardKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(action);
            break;
            case InputManager.Actions.Left:
                leftKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(action);
            break;
            case InputManager.Actions.Right:
                rightKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(action);
            break;
            case InputManager.Actions.Jump:
                jumpKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(action);
            break;
            case InputManager.Actions.Crouch:
                crouchKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(action);
            break;
            case InputManager.Actions.Run:
                runKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(action);
            break;
            case InputManager.Actions.Dash:
                dashKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(action);
            break;
            case InputManager.Actions.Attack:
                attackKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(action);
            break;
            case InputManager.Actions.TogglePause:
                togglePauseKeyButtonText.text = InputManager.Instance.UpdateKeyButtonText(action);
            break;
        }
    }
}


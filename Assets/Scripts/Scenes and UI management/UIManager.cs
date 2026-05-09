using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}
    [SerializeField] PauseUI pauseUI;
    [SerializeField] KeyRebindUI keyRebindUI;
    [SerializeField] GameObject rebindingKeyUI;
    [SerializeField] CameraUI cameraUI;
    void Start()
    {
        keyRebindUI.OnRebindStateChanged += KeyRebindUI_RebindStateChanged;
        keyRebindUI.OnReturnedToPauseMenu += KeyRebindUI_ReturnToPauseMenu;
        GameManager.Instance.OnPauseToggled += GameManager_PauseToggled;
        pauseUI.OnKeyRebindMenuOppened += PauseUI_OpenKeyRebindMenu;
        pauseUI.OnPauseMenuClosed += PauseUI_ClosePauseMenu;
        pauseUI.OnCameraUIOppened += PauseUI_OpenCameraUI;
        cameraUI.OnReturnedToPauseMenu += CameraUI_ReturnToPauseMenu;
    }
    void KeyRebindUI_RebindStateChanged(object sender, bool e)
    {
        if (e)
        {
            rebindingKeyUI.SetActive(true);
        }
        else
        {
            rebindingKeyUI.SetActive(false);
        }
    }
    void KeyRebindUI_ReturnToPauseMenu(object sender, EventArgs e)
    {
        keyRebindUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(true);
    }
    void CameraUI_ReturnToPauseMenu(object sender, EventArgs e)
    {
        cameraUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(true);
    }
    void GameManager_PauseToggled(object sender, bool e)
    {
        pauseUI.gameObject.SetActive(e);
        if (keyRebindUI.gameObject.activeInHierarchy) keyRebindUI.gameObject.SetActive(false);  
        if (cameraUI.gameObject.activeInHierarchy) cameraUI.gameObject.SetActive(false);  
    }
    void PauseUI_OpenCameraUI(object sender, EventArgs e)
    {
        pauseUI.gameObject.SetActive(false);
        cameraUI.gameObject.SetActive(true);
    }
    void PauseUI_OpenKeyRebindMenu(object sender, EventArgs e)
    {
        pauseUI.gameObject.SetActive(false);
        keyRebindUI.gameObject.SetActive(true);
    }
    void PauseUI_ClosePauseMenu(object sender, EventArgs e)
    {
        pauseUI.gameObject.SetActive(false);
        GameManager.Instance.SetPausedState(false);
        Time.timeScale = 1;
    }
}

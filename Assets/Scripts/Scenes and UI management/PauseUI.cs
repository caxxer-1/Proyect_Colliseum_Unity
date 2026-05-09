using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public event EventHandler OnKeyRebindMenuOppened;
    public event EventHandler OnCameraUIOppened;
    public event EventHandler OnPauseMenuClosed;
    [SerializeField] Button resumeButton;
    [SerializeField ] Button keyRebindButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button cameraButton;

    void Start()
    {
        resumeButton.onClick.AddListener(() => {
            OnPauseMenuClosed?.Invoke(this, EventArgs.Empty);
        });
        keyRebindButton.onClick.AddListener(() => {
            OnKeyRebindMenuOppened?.Invoke(this, EventArgs.Empty);
        });
        mainMenuButton.onClick.AddListener(() => {
            Time.timeScale = 1;
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        });
        cameraButton.onClick.AddListener(() =>
        {
            OnCameraUIOppened?.Invoke(this, EventArgs.Empty);
        });
    }
}

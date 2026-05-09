using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public event EventHandler<bool> OnPauseToggled;
    bool paused = false;
    
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
        InputManager.Instance.OnTogglePausePerformed += InputManager_TogglePause;
        
    }
    void InputManager_TogglePause(object sender, EventArgs e)
    {
        if (paused) {
            Time.timeScale = 1;
            OnPauseToggled?.Invoke(this, false);}
        else {
            Time.timeScale = 0;
            OnPauseToggled?.Invoke(this, true);}
        paused = !paused;
    }
    public void SetPausedState(bool state)
    {
        if (state) paused = true;
        else paused = false;
    }
}

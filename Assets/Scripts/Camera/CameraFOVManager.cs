using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFOVManager : MonoBehaviour
{
    float sensitivity = 1;
    [SerializeField] CinemachineVirtualCamera vCamera;
    float currentScrollValue;
    void Start()
    {
        InputManager.Instance.OnChangeFOVPerformed += InputManager_ChangeCameraFOV;
    }
    void InputManager_ChangeCameraFOV(object sender, EventArgs e)
    {
        currentScrollValue = -Mouse.current.scroll.y.ReadValue();
        float FOVChanger = currentScrollValue * sensitivity;
        vCamera.m_Lens.FieldOfView = Math.Clamp(vCamera.m_Lens.FieldOfView + FOVChanger, 50, 120);
    }
    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }
    public float GetSensitivity()
    {
        return sensitivity;
    }
}
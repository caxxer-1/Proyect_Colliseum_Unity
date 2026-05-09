using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDragMovementManager : MonoBehaviour
{
    float sensitivity = .1f;
    [SerializeField] CinemachineVirtualCamera vCamera;
    CinemachinePOV vCameraPov;
    bool dragging = false;
    void Start()
    {
        InputManager.Instance.OnCameraDragPerformed += InputManager_CameraDrag;
        InputManager.Instance.OnCameraDragCanceled += InputManager_CameraDragStopped;
        vCameraPov = vCamera.GetCinemachineComponent<CinemachinePOV>();
    }
    void Update()
    {
        if (!dragging) return;
        vCameraPov.m_HorizontalAxis.Value += Mouse.current.delta.x.ReadValue() * sensitivity;
        vCameraPov.m_VerticalAxis.Value -= Mouse.current.delta.y.ReadValue() * sensitivity;
    }
    void InputManager_CameraDrag(object sender, EventArgs e)
    {
        dragging = true;
    }
    void InputManager_CameraDragStopped(object sender, EventArgs e)
    {
        dragging = false;
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
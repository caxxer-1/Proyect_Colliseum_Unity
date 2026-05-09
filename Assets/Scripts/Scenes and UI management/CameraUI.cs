using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : MonoBehaviour
{
    public event EventHandler OnReturnedToPauseMenu;
    [SerializeField] Slider dragSensitivity;
    [SerializeField] Slider FOVSensitivity;
    [SerializeField] Button returnPauseMenuButton;
    [SerializeField] TMP_Text dragSensitivityValueText;
    [SerializeField] TMP_Text FOVSensitivityValueText;
    [SerializeField] CameraDragMovementManager cameraDragMovementManager;
    [SerializeField] CameraFOVManager cameraFOVManager;
    float cameraDragSensitivityMin = .1f;
    float cameraDragSensitivityMax = 2;
    float cameraFOVSensitivityMin = 1;
    float cameraFOVSensitivityMax = 4;
    void Start()
    {
        dragSensitivity.value = (cameraDragMovementManager.GetSensitivity() - cameraDragSensitivityMin)/(cameraDragSensitivityMin - cameraDragSensitivityMax);
        FOVSensitivity.value = (cameraFOVManager.GetSensitivity() - cameraFOVSensitivityMin)/(cameraFOVSensitivityMin - cameraFOVSensitivityMax);
        returnPauseMenuButton.onClick.AddListener(() =>
        {
            OnReturnedToPauseMenu?.Invoke(this, EventArgs.Empty);
        });
        dragSensitivity.onValueChanged.AddListener((value) =>
        {
            dragSensitivityValueText.text = value.ToString("F2");
            cameraDragMovementManager.SetSensitivity(cameraDragSensitivityMin + (dragSensitivity.value * (cameraDragSensitivityMax - cameraDragSensitivityMin)));
        });
        FOVSensitivity.onValueChanged.AddListener((value) =>
        {
            FOVSensitivityValueText.text = value.ToString("F2");
            cameraDragMovementManager.SetSensitivity(cameraFOVSensitivityMin + (FOVSensitivity.value * (cameraFOVSensitivityMax - cameraFOVSensitivityMin)));
        });
    }

}
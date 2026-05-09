using UnityEngine;

public class LookAt : MonoBehaviour
{
    enum LookAtState
    {
        LookAtCamera,
        LookAtCameraInverted,
        CameraForward,
        CameraForwardInverted
    }
    [SerializeField] LookAtState lookAt;

    void Update()
    {
        switch (lookAt)
        {
            case LookAtState.LookAtCamera:
                transform.LookAt(Camera.main.transform.position);
            break;
            case LookAtState.LookAtCameraInverted:
                transform.LookAt(-Camera.main.transform.position);
            break;
            case LookAtState.CameraForward:
                transform.forward = Camera.main.transform.forward;
            break;
            case LookAtState.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
            break;
        }
    }
}

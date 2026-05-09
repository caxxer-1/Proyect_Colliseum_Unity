using UnityEngine;

public class LoadingSceneCallback : MonoBehaviour
{
    bool isFirstUpdate = true;
    void Update()
    {
        if (!isFirstUpdate) return;
        SceneLoader.LoadTargetScene();
        isFirstUpdate = false;
    }
}
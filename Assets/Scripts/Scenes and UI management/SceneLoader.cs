using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene
    {
        MainMenu,
        CommonMap,
        MetalicMap,
        DirtMap,
        BricksMap
    }
    static Scene sceneToLoad;
    public static void LoadScene(Scene targetScene)
    {
        sceneToLoad = targetScene;
        SceneManager.LoadScene("LoadingScene");
    }
    public static void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }
}
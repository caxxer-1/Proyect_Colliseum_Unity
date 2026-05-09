using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button commonMapButton;
    [SerializeField] Button metalicMapButton;
    [SerializeField] Button dirtMapButton;
    [SerializeField] Button bricksMapButton;
    [SerializeField]  TMP_Text selectMapText;
    [SerializeField]  TMP_Text gameTitleText;
    void Start()
    {
        playButton.onClick.AddListener(() => {
            commonMapButton.gameObject.SetActive(true);
            metalicMapButton.gameObject.SetActive(true);
            dirtMapButton.gameObject.SetActive(true);
            bricksMapButton.gameObject.SetActive(true);
            selectMapText.gameObject.SetActive(true);
            gameTitleText.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
        });
        commonMapButton.onClick.AddListener(() => SceneLoader.LoadScene(SceneLoader.Scene.CommonMap));
        metalicMapButton.onClick.AddListener(() => SceneLoader.LoadScene(SceneLoader.Scene.MetalicMap));
        dirtMapButton.onClick.AddListener(() => SceneLoader.LoadScene(SceneLoader.Scene.DirtMap));
        bricksMapButton.onClick.AddListener(() => SceneLoader.LoadScene(SceneLoader.Scene.BricksMap));
        commonMapButton.gameObject.SetActive(false);
        metalicMapButton.gameObject.SetActive(false);
        dirtMapButton.gameObject.SetActive(false);
        bricksMapButton.gameObject.SetActive(false);
        selectMapText.gameObject.SetActive(false);
    }
}
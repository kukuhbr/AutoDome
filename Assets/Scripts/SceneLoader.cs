using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader sceneLoader;
    public GameObject loadingScreen;
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;
    public SceneIndex currentScene;
    public bool isLoaded;
    private void Awake()
    {
        sceneLoader = this;
    }
    private void Start()
    {
        loadingScreen.gameObject.SetActive(true);
        isLoaded = false;
        SceneManager.LoadScene((int)SceneIndex.MAIN_MENU, LoadSceneMode.Additive);
        loadingScreen.gameObject.SetActive(false);
        isLoaded = true;
        currentScene = SceneIndex.MAIN_MENU;
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadScene(SceneIndex targetScene)
    {
        loadingScreen.gameObject.SetActive(true);
        isLoaded = false;
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)currentScene));
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync((int)targetScene, LoadSceneMode.Additive);
        asyncOp.completed += (AsyncOperation o) =>
        {
            currentScene = targetScene;
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)currentScene));
            Debug.Log("The current active scene is: " + SceneManager.GetActiveScene().buildIndex);
        };


        StartCoroutine(GetSceneLoadProgress(targetScene));
    }

    float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress(SceneIndex targetScene)
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach(AsyncOperation op in scenesLoading)
                {
                    totalSceneProgress += op.progress;
                }
                totalSceneProgress = (totalSceneProgress / scenesLoading.Count);
                loadingBar.value = Mathf.Clamp(totalSceneProgress, 0f, 1f);
                loadingText.text = string.Format("Loading {0:0.00}%: {1}", totalSceneProgress*100f, targetScene.ToString());
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.3f);

        loadingScreen.gameObject.SetActive(false);
        isLoaded = true;
    }


}

public enum SceneIndex
{
    PRELOAD = 0,
    MAIN_MENU = 1,
    BATTLE_SOLO = 2,
}

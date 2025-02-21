using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ASyncloader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private Slider LoadingSlider;

    public void LoadLevelBtn(string levelToLoad)
    {
        mainMenu.SetActive(false);
        LoadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(levelToLoad));
    }
    
    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while(!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(LoadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }

}

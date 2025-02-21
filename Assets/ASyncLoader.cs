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

        // Run the A Sync
    }
    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    static GameObject mainMenu;
    static GameObject deathScreen;
    const int PlayScene = 1;
    const int MenuScene = 0;


    void Awake()
    {
        deathScreen = transform.Find("DeathUI").gameObject;
        mainMenu = transform.Find("MainUI").gameObject;
        deathScreen.SetActive(false);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void TravelToGame()
    {
        if(mainMenu)    mainMenu.SetActive(false);
        if(deathScreen) deathScreen.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex != PlayScene)
        {
            SceneManager.LoadScene(PlayScene);
        }
    }
    public static void TravelToMainMenu()
    {
        if(SceneManager.GetActiveScene().buildIndex != MenuScene)
        {
            SceneManager.LoadScene(MenuScene);
        }
        if (deathScreen) deathScreen.SetActive(false);
        if (mainMenu) mainMenu.SetActive(true);
    }
    public static void TravelToDeathScreen()
    {
        if (SceneManager.GetActiveScene().buildIndex != MenuScene)
        {
            SceneManager.LoadScene(MenuScene);
        }
        if (mainMenu)  mainMenu.SetActive(false);
        if(deathScreen) deathScreen.SetActive(true);
    }
}

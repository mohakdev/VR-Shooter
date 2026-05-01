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
        deathScreen = GameObject.FindGameObjectWithTag("DeathUI");
        mainMenu = GameObject.FindGameObjectWithTag("MainUI");
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
        SceneManager.LoadScene(PlayScene);
    }
    public static void TravelToMainMenu()
    {
        if(SceneManager.GetActiveScene().buildIndex != MenuScene)
        {
            SceneManager.LoadScene(MenuScene);
        }
        deathScreen.SetActive(false);
        mainMenu.SetActive(true);
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

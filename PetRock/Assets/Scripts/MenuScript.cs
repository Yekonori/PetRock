using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void MainMenuButtonPress()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartButtonPress()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitButtonPress()
    {
        Application.Quit();
    }

   
}

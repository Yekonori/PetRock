using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

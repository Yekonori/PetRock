using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup mainMenuPanel;
    [SerializeField]
    private CanvasGroup creditsPanel;

    private GameObject lastButtonSelected;

    public static MainMenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetMainMenu();
    }

    #region Buttons function

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsGame()
    {
        mainMenuPanel.DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            mainMenuPanel.gameObject.SetActive(false);
            creditsPanel.gameObject.SetActive(true);
            creditsPanel.DOFade(1.0f, 1.0f);
        });
    }

    public void Back()
    {
        creditsPanel.DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            creditsPanel.gameObject.SetActive(false);
            mainMenuPanel.gameObject.SetActive(true);
            mainMenuPanel.DOFade(1.0f, 1.0f);
        });
    }

    public void QuitGame()
    {
       #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    #endregion

    #region Menu functions
    private void SetMainMenu()
    {
        mainMenuPanel.DOFade(1f, 0.1f);
        creditsPanel.DOFade(0f, 0.1f);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            lastButtonSelected = EventSystem.current.currentSelectedGameObject;

        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(lastButtonSelected);

        PressedSelectedButton();
    }

    private void PressedSelectedButton()
    {
        if (Input.GetKeyUp("joystick button 1"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }
    #endregion
}

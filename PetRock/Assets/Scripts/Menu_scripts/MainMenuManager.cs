using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    #region Script parameters

    [Header("Buttons")]
    [SerializeField]
    private List<Button> _menuButtons = new List<Button>();
    [SerializeField]
    private List<Button> _backMenuButtons = new List<Button>();

    #endregion

    [Header("Panels")]
    [SerializeField]
    private CanvasGroup _mainMenuPanel;
    [SerializeField]
    private CanvasGroup _optionsPanel;
    [SerializeField]
    private CanvasGroup _creditsPanel;

    [Header("Animator")]
    [SerializeField]
    private Animator _creditsAnim;

    private GameObject _lastButtonSelected;
    private Image _backgroundMainMenu;
    private CanvasGroup _menuPanels;

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

        _backgroundMainMenu = GetComponent<Image>();
        _menuPanels = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        SetMenuButtons();
        SetBackMenuButtons();
    }

    #region Buttons function

    void PlayGame()
    {
        _menuPanels.DOFade(0, 1).OnComplete(() =>
        {
            GameManager.instance.inMainMenu = false;
            Destroy(gameObject);
        });
    }

    void OptionsGame()
    {
        PanelSwitch(_mainMenuPanel, _optionsPanel);
    }

    void CreditsGame()
    {
        PanelSwitch(_mainMenuPanel, _creditsPanel);
        _backgroundMainMenu.DOFade(1, 1).OnComplete(() => _creditsAnim.SetBool("PlayCredits", true));
    }

    void BackCredits()
    {
        PanelSwitch(_creditsPanel, _mainMenuPanel);
        _backgroundMainMenu.DOFade(0.5f, 1);
    }

    void BackOptions()
    {
        PanelSwitch(_optionsPanel, _mainMenuPanel);
    }

    void PanelSwitch(CanvasGroup firstPanel, CanvasGroup secondPanel)
    {
        firstPanel.DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            firstPanel.gameObject.SetActive(false);
            secondPanel.gameObject.SetActive(true);
            secondPanel.DOFade(1.0f, 1.0f);
        });
    }

    void QuitGame()
    {
       #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    #endregion

    #region Menu functions

    private void SetMenuButtons()
    {
        foreach (Button button in _menuButtons)
            button.onClick.RemoveAllListeners();

        _menuButtons[0].onClick.AddListener(PlayGame); //Play button
        _menuButtons[1].onClick.AddListener(OptionsGame); //Options button
        _menuButtons[2].onClick.AddListener(CreditsGame); //Credits button
        _menuButtons[3].onClick.AddListener(QuitGame); //Quit button
    }

    private void SetBackMenuButtons()
    {
        foreach(Button button in _backMenuButtons)
            button.onClick.RemoveAllListeners();

        _backMenuButtons[0].onClick.AddListener(BackOptions); //Options Button
        _backMenuButtons[1].onClick.AddListener(BackCredits); //Credits button
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            _lastButtonSelected = EventSystem.current.currentSelectedGameObject;

        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(_lastButtonSelected);

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

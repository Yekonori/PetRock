using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu_Scripts : MonoBehaviour
{
    #region Script parameters

    [SerializeField]
    private List<Button> _menuPauseButtons = new List<Button>();

    private GameObject _lastButtonSelected;

    private bool _isOpen = false;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip _resumeGameClip;
    [SerializeField]
    private AudioClip _restartGameClip;

    #endregion

    private void Awake()
    {
        _isOpen = true;
        transform.DOLocalMove(Vector3.zero, 1.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            DOTween.To(() => GameManager.instance.dofPostProcessing.focusDistance.value, x => GameManager.instance.dofPostProcessing.focusDistance.value = x, 0.1f, 0.5f).OnComplete(()=>
            {
                Time.timeScale = 0.0f;
                EventSystem.current.SetSelectedGameObject(_menuPauseButtons[0].gameObject);
            });
        });
        SetMenuButtons();
    }

    #region Buttons function

    private void ClosePauseMenu(Button_Script button)
    {
        Time.timeScale = 1.0f;
        button.GetAudioSource().PlayOneShot(_resumeGameClip);
        _isOpen = false;
        EventSystem.current.SetSelectedGameObject(null);
        transform.DOLocalMoveX(1500f, 1.5f).SetEase(Ease.InBack).OnComplete(() => 
        {
            DOTween.To(() => GameManager.instance.dofPostProcessing.focusDistance.value, x => GameManager.instance.dofPostProcessing.focusDistance.value = x, 10.0f, 0.5f);
            GameManager.instance._inPause = false;
            Destroy(gameObject); 
        });
    }

    private void RestartLevel(Button_Script button)
    {
        Time.timeScale = 1.0f;
        button.GetAudioSource().PlayOneShot(_restartGameClip);
        GameManager.instance.TransitionCanvas(1).OnComplete(() =>
        {
            GameManager.instance._inPause = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    private void QuitGame()
    {

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    #endregion

    #region Pause menu functions    

    private void SetMenuButtons()
    {
        foreach (Button button in _menuPauseButtons)
            button.onClick.RemoveAllListeners();

        _menuPauseButtons[0].onClick.AddListener(() => ClosePauseMenu(_menuPauseButtons[0].gameObject.GetComponent<Button_Script>())); //Resume button
        _menuPauseButtons[1].onClick.AddListener(() => RestartLevel(_menuPauseButtons[1].gameObject.GetComponent<Button_Script>())); //Restart button
        _menuPauseButtons[2].onClick.AddListener(QuitGame); //Quit button
    }

    private void Update()
    {
        if (_isOpen)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                _lastButtonSelected = EventSystem.current.currentSelectedGameObject;

            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(_lastButtonSelected);
        }

        PressedSelectedButton();
    }

    private void PressedSelectedButton()
    {
        if (GameManager.instance.player.GetButton("PressButton"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    #endregion
}

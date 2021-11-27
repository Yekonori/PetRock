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

    #endregion

    private void Awake()
    {
        _isOpen = true;
        transform.DOLocalMove(Vector3.zero, 1.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Time.timeScale = 0.0f;
            EventSystem.current.SetSelectedGameObject(_menuPauseButtons[0].gameObject);
        });
        SetMenuButtons();
    }

    #region Buttons function

    private void ClosePauseMenu()
    {
        Time.timeScale = 1.0f;
        _isOpen = false;
        EventSystem.current.SetSelectedGameObject(null);
        transform.DOLocalMoveX(1500f, 1.5f).SetEase(Ease.InBack).OnComplete(() => 
        {
            GameManager.instance._inPause = false;
            Destroy(gameObject); 
        });
    }

    private void RestartLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        _menuPauseButtons[0].onClick.AddListener(ClosePauseMenu); //Resume button
        _menuPauseButtons[1].onClick.AddListener(RestartLevel); //Restart button
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
        if (Input.GetKeyUp("joystick button 1"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    #endregion
}

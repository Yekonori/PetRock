using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    [BoxGroup("Pause Menu Paramaters")]
    public GameObject _pauseMenu;
    [HideInInspector]
    public bool _inPause = false;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PauseGame()
    {
        _inPause = true;
        GameObject pMenu = Instantiate(_pauseMenu, GameObject.FindGameObjectWithTag("Canvas").transform);
        pMenu.transform.localPosition = new Vector3(1500, 0, 0);
    }
}

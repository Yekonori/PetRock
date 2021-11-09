using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Rewired;

public class GameManager : MonoBehaviour
{
    [BoxGroup("Pause Menu Paramaters")]
    public GameObject _pauseMenu;
    [HideInInspector]
    public bool _inPause = false;

    [HideInInspector]
    public bool inRockBalancing = false;

    [TitleGroup("Post-Processing")]
    [SerializeField]
    private Volume volumePostProcessing;
    [HideInInspector]
    public Vignette vignettePostProcessing;

    [HideInInspector]
    public Player player;

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

        SetVignettePostProcess();
    }

    #region MenuManager

    public void PauseGame()
    {
        _inPause = true;
        GameObject pMenu = Instantiate(_pauseMenu, GameObject.FindGameObjectWithTag("Canvas").transform);
        pMenu.transform.localPosition = new Vector3(1500, 0, 0);
    }

    #endregion MenuManager

    #region PostProcessManager

    private void SetVignettePostProcess()
    {
        Vignette vignetteTmp;

        if (volumePostProcessing.profile.TryGet(out vignetteTmp))
        {
            vignettePostProcessing = vignetteTmp;
        }
    }

    #endregion PostProcessManager
}

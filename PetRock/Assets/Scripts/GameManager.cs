using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Rewired;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [BoxGroup("Pause Menu Paramaters")]
    public GameObject _pauseMenu;
    [HideInInspector]
    public bool _inPause = false;

    [HideInInspector]
    public bool inRockBalancing = false;
    [HideInInspector]
    public bool startDialogueRB = false;
    //[HideInInspector]
    public bool inMainMenu = false;

    [TitleGroup("Post-Processing")]
    [SerializeField]
    private Volume volumePostProcessing;
    [HideInInspector]
    public Vignette vignettePostProcessing;

    [TitleGroup("Canvas")]
    [SerializeField]
    private CanvasGroup _transitionCanvas;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TransitionCanvas(0).SetDelay(1);
    }

    public DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> TransitionCanvas(float goTo)
    {
        return _transitionCanvas.DOFade(goTo, 2f);
    }

    public IEnumerator startRB(GameObject go)
    {
        if(_transitionCanvas.alpha >= 1)
        {
            TransitionCanvas(0);
        }

        inRockBalancing = true;
        PlayerParameters.Instance.UpdateRockBalancing(inRockBalancing);

        go.GetComponent<RockBalancing_Dialogue>().StartDialogue();

        yield return new WaitWhile(() => go.GetComponent<RockBalancing_Dialogue>().CheckEndDialogue());
        TransitionCanvas(1).OnComplete(() =>
        {
            PlayerParameters.Instance.gameObject.transform.parent = go.GetComponent<RockBalancingScript>().transform;

            PlayerParameters.Instance.gameObject.transform.localPosition = go.GetComponent<RockBalancingScript>().startPlayerPos.localPosition;
            PlayerParameters.Instance.gameObject.transform.rotation = go.GetComponent<RockBalancingScript>().startPlayerPos.rotation;

            PlayerParameters.Instance.anim.SetBool("rockBalance", true);
            go.GetComponent<TriggeredViewVolume>().ActiveView(true);
            go.GetComponent<RockBalancingScript>().enabled = true;
            TransitionCanvas(0).SetDelay(1);
        });
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

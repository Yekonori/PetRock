using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Rewired;
using DG.Tweening;
using TMPro;

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
    [HideInInspector]
    public DepthOfField dofPostProcessing;

    [TitleGroup("Canvas")]
    [SerializeField]
    private Transition_Script _transitionCanvas;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_transitionCanvas == null)
            _transitionCanvas = GameObject.FindGameObjectWithTag("TransitionCanvas").GetComponent<Transition_Script>();

        volumePostProcessing = GameObject.FindGameObjectWithTag("PostProcess").GetComponent<Volume>();

        SetVignettePostProcess();
        SetDofPostProcess();

        if(player != null)
        {
            player.SetVibration(0, 0);
            player.StopVibration();
        }

        inRockBalancing = false;

        TransitionCanvas(0).SetDelay(1);
    }

    public DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> TransitionCanvas(float goTo)
    {
        return _transitionCanvas.transitionCanvas.DOFade(goTo, 2f);
    }
    
    public DG.Tweening.Core.TweenerCore<UnityEngine.Color, UnityEngine.Color, DG.Tweening.Plugins.Options.ColorOptions> TransitionTextDialogue(float goTo)
    {
        return _transitionCanvas.gameObject.GetComponentInChildren<TextMeshProUGUI>().DOFade(goTo, 2f);
    }

    public IEnumerator startRB(GameObject go)
    {
        if(_transitionCanvas.transitionCanvas.alpha >= 1)
        {
            TransitionCanvas(0);
        }

        inRockBalancing = true;
        PlayerParameters.Instance.UpdateRockBalancing(inRockBalancing);

        go.GetComponent<RockBalancing_Dialogue>().StartDialogue();

        yield return new WaitWhile(() => go.GetComponent<RockBalancing_Dialogue>().CheckEndDialogue());
        TransitionCanvas(1).OnComplete(() =>
        {
            PlayerParameters.Instance.gameObject.GetComponent<Player_PetRock>().petRock.SetActive(false);

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

        player.SetVibration(0, 0);
        player.StopVibration();

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

    private void SetDofPostProcess()
    {
        DepthOfField depthOfFieldTemp;

        if(volumePostProcessing.profile.TryGet(out depthOfFieldTemp))
        {
            dofPostProcessing = depthOfFieldTemp;
        }
    }

    #endregion PostProcessManager
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeOutManager : MonoBehaviour
{
    [Header("Gauge")]
    [SerializeField]
    private float backToTimeOutValue;

    [Header("Dialogue")]
    [SerializeField]
    private DialogueScript _ConversationTO;

    [SerializeField]
    private DialogDisplay _dialogDisplay;

    //GameManager _gameManager;
    [SerializeField]
    private GameObject _timeOutCam;

    public static TimeOutManager instance;

    // Start is called before the first frame update
    void Awake()
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

        //_gameManager = GameManager.instance;
    }

    public IEnumerator TimeOut() 
    {
        PlayerParameters.Instance.anim.SetBool("InTimeOut", true);

        yield return new WaitForSeconds(1.5f);

        GameManager.instance.TransitionCanvas(1).OnComplete(() =>
        {
            _timeOutCam.SetActive(true);
            GameManager.instance.TransitionCanvas(0).OnComplete(() =>
            {
                _dialogDisplay.SetThisDialogTex(_ConversationTO);
                _dialogDisplay.DisplayDialog();
                _dialogDisplay.NextDialog();
            });
        });

        yield return new WaitForSeconds(6);

        yield return new WaitWhile(() => _dialogDisplay.activeDialog);

        EndTimeOut();
    }

    void EndTimeOut()
    {
        GameManager.instance.TransitionCanvas(1).OnComplete(() =>
        {
            PlayerParameters.Instance.anim.SetBool("InTimeOut", false);
            _timeOutCam.SetActive(false);
            GameManager.instance.TransitionCanvas(0).SetDelay(1).OnComplete(()=>
            {
                PlayerParameters.Instance.UpdateStateGauge(backToTimeOutValue);
                PlayerParameters.Instance.UpdatePlayerState(PlayerParameters.PlayerStates.Regular);
            });
        });
    }
}

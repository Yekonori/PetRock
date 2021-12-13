using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class Trigger_Dialog : TriggeredViewVolume
{
    public DialogueScript conversation;
    public DialogDisplay dialogDisplay;

    [Header("Positions")]
    [SerializeField] Transform posPlayer;
    [SerializeField] Transform posRock;
    [SerializeField] Transform posMiddle;

    [Header("Next Scene")]
    [SerializeField] int nextScene = -1;

    [Header("Transition")]
    [SerializeField]
    private bool _startTransition = false;
    [SerializeField]
    private bool _endTransition = false;

    [Header("Animation")]
    [SerializeField]
    private bool _hasAnim = false;
    [SerializeField, ShowIf("@_hasAnim == true")]
    private string animParam;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().SetInDialog(true);

            if (_startTransition)
            {
                GameManager.instance.TransitionCanvas(1).OnComplete(() =>
                {
                    StartDialogue(other);
                    GameManager.instance.TransitionCanvas(0).SetDelay(1);
                });
            }
            else
            {
                StartDialogue(other);
            }
        }
    }

    void StartDialogue(Collider other)
    {
        if (_hasAnim && animParam != null)
            PlayerParameters.Instance.anim.SetBool(animParam, true);

        if (view is FixedFollowView)
        {
            dialogDisplay.SetView((FixedFollowView)view, posPlayer, posRock, posMiddle);
        }

        dialogDisplay.SetThisDialogTex(conversation);
        dialogDisplay.DisplayDialog();
        dialogDisplay.NextDialog();

        if (conversation.automatic)
        {
            Destroy(this.gameObject);
        }
        else
        {
            other.transform.position = posPlayer.position;
            other.transform.rotation = posPlayer.rotation;
            Physics.SyncTransforms();

            RockScript rock = other.GetComponentInChildren<RockScript>();
            rock.MoveTotarget(posRock);

            SetActive(true);

            dialogDisplay.SetEndAction(() => 
            {
                if (_endTransition)
                {
                    GameManager.instance.TransitionCanvas(1).OnComplete(() =>
                    {
                        if (_hasAnim && animParam != null)
                            PlayerParameters.Instance.anim.SetBool(animParam, false);

                        GameManager.instance.TransitionCanvas(0).SetDelay(1).OnComplete(()=> 
                        {
                            other.gameObject.GetComponent<PlayerMovement>().SetInDialog(false);
                        });

                        SetActive(false);
                    });
                }
                else
                {
                    if (_hasAnim && animParam != null && nextScene == -1)
                        PlayerParameters.Instance.anim.SetBool(animParam, false);

                    SetActive(false);
                }
            });

            dialogDisplay.SetEndAction(() => rock.ResetPosition());
            dialogDisplay.SetEndAction(() => Destroy(this.gameObject));

            if (nextScene != -1)
            {
                dialogDisplay.SetEndAction(() =>
                {
                    GameManager.instance.TransitionCanvas(1).OnComplete(() =>
                    {
                        GameManager.instance.TransitionTextDialogue(1).OnPlay(() => DOTween.To(() => AudioListener.volume, x => AudioListener.volume = x, 0, 2)).OnComplete(() =>
                        {
                            GameManager.instance.TransitionTextDialogue(0).SetDelay(5).OnComplete(() =>
                            {
                                SceneManager.LoadScene(nextScene);
                            });
                        });
                    });
                });
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}

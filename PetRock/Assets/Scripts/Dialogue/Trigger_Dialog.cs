using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
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

            dialogDisplay.SetEndAction(() => SetActive(false));
            dialogDisplay.SetEndAction(() => rock.ResetPosition());
            dialogDisplay.SetEndAction(() => Destroy(this.gameObject));

            if (nextScene != -1)
            {
                dialogDisplay.SetEndAction(() =>
                {
                    GameManager.instance.TransitionCanvas(1).OnComplete(() =>
                    {
                        SceneManager.LoadScene(nextScene);
                    });
                });
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}

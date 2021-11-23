using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Dialog : TriggeredViewVolume
{
    public DialogueScript conversation;
    public DialogDisplay dialogDisplay;

    [Header("Positions")]
    [SerializeField] Transform posPlayer;
    [SerializeField] Transform posRock;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
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

                posRock.gameObject.SetActive(true);

                SetActive(true);

                dialogDisplay.SetEndAction(() => SetActive(false));
                dialogDisplay.SetEndAction(() => Destroy(this.gameObject));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}

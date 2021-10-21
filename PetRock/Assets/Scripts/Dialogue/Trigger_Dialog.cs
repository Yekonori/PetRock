using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Dialog : MonoBehaviour
{

    public DialogueScript conversation;
    public DialogDisplay dialogDisplay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dialogDisplay.SetThisDialogTex(conversation);
            dialogDisplay.DisplayDialog();
            dialogDisplay.NextDialog();
        }
    }
}

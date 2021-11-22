using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBalancing_Dialogue : MonoBehaviour
{
    public DialogueScript startConversationRB;
    public DialogueScript endConversationRB;

    public DialogDisplay dialogDisplay;

    public void StartDialogue()
    {
        Dialogue(startConversationRB);
    }

    public void EndDialogue()
    {
        Dialogue(endConversationRB);
    }

    void Dialogue(DialogueScript dialogueScript)
    {
        dialogDisplay.SetThisDialogTex(dialogueScript);
        dialogDisplay.DisplayDialog();
        dialogDisplay.NextDialog();
    }

    public bool CheckEndDialogue()
    {
        return dialogDisplay.activeDialog;
    }
}

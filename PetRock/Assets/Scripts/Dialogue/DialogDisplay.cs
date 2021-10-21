using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDisplay : MonoBehaviour
{
    public DialogueScript conversation;

    public GameObject speakerLeft;
    public GameObject speakerRight;
    //public GameObject dialogObject;

    private int activeLineIndex = 0;
   
    private DialogueUI speakerUILeft;
    private DialogueUI speakerUIRight; 
    //private DialogueUI dialogUI;

    private bool activeDialog;

    // Start is called before the first frame update
    void Start()
    {
        speakerUILeft = speakerLeft.GetComponent<DialogueUI>();
        speakerUIRight = speakerRight.GetComponent<DialogueUI>();
        //dialogUI = dialogObject.GetComponent<DialogueUI>();

        speakerUILeft.Speaker = conversation.leftCharacter;
        speakerUIRight.Speaker = conversation.rightCharacter;

        activeDialog = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            NextDialog();
        }
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            DisplayDialog();
        NextDialog();
        }*/
    }

    //Passe au texte suivant
    public void NextDialog()
    {
        if(activeLineIndex < conversation.lines.Length && activeDialog)
        {
            DisplayLine();
            activeLineIndex += 1;
        }
        else
        {
            speakerUILeft.Hide();
            speakerUIRight.Hide();
            //dialogUI.Hide();

            activeDialog = false;
            activeLineIndex = 0;
        }
    }

    public void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        Characters character = line.character;

        if(speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line.text);
        }
        else
        {
            SetDialog(speakerUIRight, speakerUILeft, line.text);
        }
        //SetDialog(line.text);
    }

    void SetDialog(DialogueUI activeSpeakerUI, DialogueUI inactiveSpeakerUI, string text)
    {
        activeSpeakerUI.Dialog = text;
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();

        /*
        dialogUI.Dialog = text;
        dialogUI.Show();*/
    }

    //Active la boîte de dialogue
    public void DisplayDialog()
    {
        activeDialog = true;
    }

    public void SetThisDialogTex(DialogueScript dialogText)
    {
        conversation = dialogText;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDisplay : MonoBehaviour
{
    public DialogueScript conversation;

    public GameObject dialogObject;

    private int activeLineIndex = 0;
    private DialogueUI dialogUI;

    // Start is called before the first frame update
    void Start()
    {
        dialogUI = dialogObject.GetComponent<DialogueUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            NextDialog();
        }
    }

    //Passe au texte suivant
    public void NextDialog()
    {
        if(activeLineIndex < conversation.lines.Length)
        {
            DisplayLine();
            activeLineIndex += 1;
        }
        else
        {
            dialogUI.Hide();
            activeLineIndex = 0;
        }
    }

    public void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];

        SetDialog(line.text);
    }

    void SetDialog(string text)
    {
        dialogUI.Dialog = text;
        dialogUI.Show();
    }

}

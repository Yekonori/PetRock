using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public struct Line
{
    public Characters character;

    [TextArea(2, 5)]
    public string text;
    public bool focusCharacter;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueScript : ScriptableObject
{
    public Characters leftCharacter;
    public Characters rightCharacter;
    public Line[] lines;
    public bool automatic;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public struct Line
{
    [TextArea(2, 5)]
    public string text;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueScript : ScriptableObject
{
    public Line[] lines;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceScript : MonoBehaviour
{


    [System.Serializable]
    public struct Choice
    {
        [TextArea(2, 5)]
        public string text;


        [CreateAssetMenu(fileName = "Text for Choices", menuName = "Text for choice")]
        public class ChoiceScript : ScriptableObject
        {
            [TextArea(2, 5)]
            public string text;

        }
    }
}

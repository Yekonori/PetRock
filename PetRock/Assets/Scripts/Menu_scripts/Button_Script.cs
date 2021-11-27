using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button_Script : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
            GetComponent<Image>().color = GetComponent<Button>().colors.selectedColor;
        else
            GetComponent<Image>().color = GetComponent<Button>().colors.normalColor;
    }
}

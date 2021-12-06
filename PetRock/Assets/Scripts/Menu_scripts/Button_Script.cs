using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class Button_Script : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (GetComponent<Image>() != null)
                GetComponent<Image>().color = GetComponent<Button>().colors.selectedColor;
            else
                GetComponent<TextMeshProUGUI>().color = GetComponent<Button>().colors.selectedColor;

            GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        else
        {
            if (GetComponent<Image>() != null)
                GetComponent<Image>().color = GetComponent<Button>().colors.normalColor;
            else
                GetComponent<TextMeshProUGUI>().color = GetComponent<Button>().colors.normalColor;

            GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 0.5f);
        }
    }
}

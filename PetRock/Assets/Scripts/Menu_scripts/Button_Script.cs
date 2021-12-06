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

            GetComponentInChildren<TextMeshProUGUI>().DOFade(1.0f, 0.5f);
        }
        else
        {
            if (GetComponent<Image>() != null)
                GetComponent<Image>().color = GetComponent<Button>().colors.normalColor;
            else
                GetComponent<TextMeshProUGUI>().color = GetComponent<Button>().colors.normalColor;

            GetComponentInChildren<TextMeshProUGUI>().DOFade(0.5f, 0.5f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Button_Script : MonoBehaviour
{
    [SerializeField]
    private bool _changeSize = false;

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (GetComponent<Image>() != null)
                GetComponent<Image>().sprite = GetComponent<Button>().spriteState.selectedSprite;
            else
                GetComponent<TextMeshProUGUI>().color = GetComponent<Button>().colors.selectedColor;

            if (_changeSize)
            {
                if (GetComponentInChildren<TextMeshProUGUI>() != null)
                    GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = 50.0f;

                if (GetComponent<RectTransform>() != null)
                    GetComponent<RectTransform>().sizeDelta = new Vector2(560.0f, 100.0f);
            }
        }
        else
        {
            if (GetComponent<Image>() != null)
                GetComponent<Image>().sprite = GetComponent<Button>().spriteState.highlightedSprite;
            else
                GetComponent<TextMeshProUGUI>().color = GetComponent<Button>().colors.normalColor;

            if (_changeSize)
            {
                if (GetComponentInChildren<TextMeshProUGUI>() != null)
                    GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = 30.0f;

                if (GetComponent<RectTransform>() != null)
                    GetComponent<RectTransform>().sizeDelta = new Vector2(400.0f, 70.0f);
            }
        }
    }
}

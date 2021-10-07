using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Panel_script : MonoBehaviour
{
    public GameObject firstButtonPanel;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButtonPanel);
    }
}

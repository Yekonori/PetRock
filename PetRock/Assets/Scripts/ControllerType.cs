using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;

public class ControllerType : MonoBehaviour
{
    [Header("Input settings")]
    [SerializeField]
    private TextMeshProUGUI settingsNextPanel;
    [SerializeField]
    private TextMeshProUGUI settingsPreviousPanel;

    private const string LB = "LB";
    private const string RB = "RB";

    private const string L1 = "L1";
    private const string R1 = "R1";

    private const string L = "L";
    private const string R = "R";

    public enum TypeController
    {
        XBox,
        Playstation,
        Switch
    }

    [Header("Controller type")]
    public TypeController typeController;

    public static ControllerType Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);
    }

    public void _GetInputTypeFromGUID(System.Guid guid)
    {
        switch (guid.ToString())
        {
            //Nintendo Switch
            case "3eb01142-da0e-4a86-8ae8-a15c2b1f2a04":
            case "605dc720-1b38-473d-a459-67d5857aa6ea":
            case "7bf3154b-9db8-4d52-950f-cd0eed8a5819":
                SetSwtich();
                break;

            //Playstation
            case "c3ad3cad-c7cf-4ca8-8c2e-e3df8d9960bb":
            case "71dfe6c8-9e81-428f-a58e-c7e664b7fbed":
            case "cd9718bf-a87a-44bc-8716-60a0def28a9f":
                SetPlaystation();
                break;

            //Xbox
            case "d74a350e-fe8b-4e9e-bbcd-efff16d34115":
            case "19002688-7406-4f4a-8340-8d25335406c8":
                SetXbox();
                break;

            default:
                SetXbox();
                break;
        }
    }

    void SetSwtich()
    {
        typeController = TypeController.Switch;

        if(MainMenuManager.instance != null)
        {
            settingsPreviousPanel.text = L;
            settingsNextPanel.text = R;
        }
    }

    void SetPlaystation()
    {
        typeController = TypeController.Playstation;

        if (MainMenuManager.instance != null)
        {
            settingsPreviousPanel.text = L1;
            settingsNextPanel.text = R1;
        }
    }

    void SetXbox()
    {
        typeController = TypeController.XBox;

        if (MainMenuManager.instance != null)
        {
            settingsPreviousPanel.text = LB;
            settingsNextPanel.text = RB;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class PlayerParameters : MonoBehaviour
{
    [PropertyRange(0,100)]
    public float panicGauge;

    [HideInInspector]
    public bool doRockBalancing = false;
    [HideInInspector]
    public bool inSafeZone = false;

    public enum PlayerStates
    {
        Regular,
        GiantZone,
        Stressed
    }

    public enum PreviousPlayerStates
    {
        Regular,
        GiantZone,
        Stressed
    }

    //[HideInInspector]
    public PreviousPlayerStates previousPlayerStates = PreviousPlayerStates.Regular;
    public PlayerStates playerStates = PlayerStates.Regular;

    public static PlayerParameters Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (panicGauge >= 100)
            SceneManager.LoadScene("Defeat_Scene");
    }

    public void UpdatePlayerState(PreviousPlayerStates tempPreviousPlayerStates, PlayerStates tempPlayerStates)
    {
        previousPlayerStates = tempPreviousPlayerStates;
        playerStates = tempPlayerStates;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class PlayerParameters : MonoBehaviour
{
    [PropertyRange(0,100)]
    public float panicGauge;

    [Header("Stress")]
    public float decreaseStressValuePerSecond = 20;

    private bool doRockBalancing = false;
    private bool inSafeZone = false;
    private float t = 0;

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

        if(inSafeZone)
        {
            t += Time.deltaTime;
            if(t>= 1)
            {
                panicGauge -= decreaseStressValuePerSecond;
                t = 0;
                if (panicGauge < 0)
                    panicGauge = 0;
            }
        }
        else
        {
            t = 0;
        }
    }

    #region Update states

    public void UpdatePlayerState(PreviousPlayerStates tempPreviousPlayerStates, PlayerStates tempPlayerStates)
    {
        previousPlayerStates = tempPreviousPlayerStates;
        playerStates = tempPlayerStates;
    }

    public void UpdateRockBalancing(bool active)
    {
        doRockBalancing = active;
    }

    public void UpdateStateSafeZone(bool active)
    {
        inSafeZone = active;
    }

    #endregion Update states

    #region Bool

    public bool IsOnSafeZone()
    {
        return inSafeZone;
    }

    public bool IsOnRockBalancing()
    {
        return doRockBalancing;
    }

    public bool HasBeenOnGiantZone()
    {
        return previousPlayerStates == PreviousPlayerStates.GiantZone;
    }

    public bool HasBeenOnStressed()
    {
        return previousPlayerStates == PreviousPlayerStates.Stressed;
    }

    #endregion Bool
}

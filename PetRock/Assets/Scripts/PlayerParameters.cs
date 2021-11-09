using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class PlayerParameters : MonoBehaviour
{
    [PropertyRange(0,100)]
    public float panicGauge;

    [SerializeField, Min(0)] float timeFromStressedToRegular = 3f;
    [SerializeField, Min(0)] float timeFromGiantToStressed = 0.3f;
    private float stateTimer = 0f;

    private bool doRockBalancing = false;
    private bool inSafeZone = false;

    public enum PlayerStates
    {
        Regular,
        GiantZone,
        Stressed
    }

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
        if (playerStates == PlayerStates.GiantZone)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > timeFromGiantToStressed)
            {
                UpdatePlayerState(PlayerStates.Stressed);
            }
        }
        else if (playerStates == PlayerStates.Stressed)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > timeFromStressedToRegular)
            {
                UpdatePlayerState(PlayerStates.Regular);
            }
        }

        if (panicGauge >= 100)
            SceneManager.LoadScene("Defeat_Scene");
    }

    #region Update states

    public void UpdatePlayerState(PlayerStates tempPlayerStates)
    {
        playerStates = tempPlayerStates;
        stateTimer = 0f;
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
        return playerStates == PlayerStates.GiantZone;
    }

    public bool HasBeenOnStressed()
    {
        return playerStates == PlayerStates.Stressed;
    }

    #endregion Bool
}

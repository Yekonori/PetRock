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

    [Header("Stress")]
    public float decreaseStressValuePerSecond = 20;

    private bool doRockBalancing = false;
    private bool inSafeZone = false;
    private float t = 0;
    private Animator anim;

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
        anim = GetComponent<Animator>();
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
                anim.SetBool("isStressed", true);
            }
        }
        else if (playerStates == PlayerStates.Stressed)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > timeFromStressedToRegular)
            {
                UpdatePlayerState(PlayerStates.Regular);
                anim.SetBool("isStressed", false);
            }
        }

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

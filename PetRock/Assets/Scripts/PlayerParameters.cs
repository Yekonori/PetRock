using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerParameters : MonoBehaviour
{
    [PropertyRange(0,100)]
    public float panicGauge;

    public static PlayerParameters Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}

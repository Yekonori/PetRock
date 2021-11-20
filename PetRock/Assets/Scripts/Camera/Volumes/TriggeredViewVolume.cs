﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredViewVolume : AViewVolume
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.tag != "RockBalancing")
        {
            SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetActive(false);
        }
    }

    public void ActiveView(bool active)
    {
        SetActive(active);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePanic : MonoBehaviour
{
    [SerializeField]
    private bool active = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PanicManager.Instance.PlayerCanPanic(active);
        }
    }
}

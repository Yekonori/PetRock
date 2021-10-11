using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum eAction
{
    None,
    Grab
}

public class ActionTrigger : MonoBehaviour
{
    [SerializeField]
    eAction action;

    [SerializeField]
    Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        if (!_collider)
            _collider = GetComponent<Collider>();
        if (!_collider)
        {
            Debug.LogError("No Collider on object");
            Application.Quit();
        }
        if (!_collider.isTrigger)
        {
            Debug.LogError("Collider is not set on trigger");
            Application.Quit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (action)
            {
                case eAction.Grab:
                    other.GetComponent<PlayerMovement>().SetCanGrab(true, transform.parent);
                    break;
                //default:
                //    Debug.Log("No action");
                //    break;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (action)
            {
                case eAction.Grab:
                    other.GetComponent<PlayerMovement>().SetCanGrab(false, transform.parent);
                    break;
                //default:
                //    Debug.Log("No action");
                //    break;
            }

        }
    }
}


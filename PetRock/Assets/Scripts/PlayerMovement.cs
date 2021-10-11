using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Script Parameters
    [Header("..")]
    [SerializeField, Min(0f)] float speed = 5f;
    [SerializeField, Min(0f)] float rotationSpeed = 90f;

    [Header("Grab")]
    [SerializeField] Transform grabPosition;
    //[SerializeField, Min(0f)] float gravity = 5f; // can we jump? fall ?

    #endregion Script Parameters

    #region Fields

    // direction and rotation
    private int _moveForward = 0;
    private int _moveRotation = 0;

    private bool _canGrab = false;
    private Transform _objectToGrab;
    private bool isGrabing = false;
    #endregion Fields

    private void Start()
    {
        if (!grabPosition)
        {
            Debug.LogError("No grab Position");
            Application.Quit();
        }
    }
    void Update()
    {
        transform.position += _moveForward * transform.forward * speed * Time.deltaTime; // += gravity * 
        transform.Rotate(Vector3.up, _moveRotation * rotationSpeed * Time.deltaTime);
    }

    public void SetMovementDirection(float vertical, float rotate)
    {
        _moveForward = (int)vertical;
        _moveRotation = (int)rotate;
    }

    public void SetCanGrab(bool canGrab, Transform objectToGrab)
    {
        _canGrab = canGrab;
        _objectToGrab = objectToGrab;
    }

    public void Grab()
    {
        if (!_canGrab || !_objectToGrab) return;

        if (!isGrabing)
        {
            isGrabing = true;
            _objectToGrab.SetParent(grabPosition);
            _objectToGrab.localPosition = Vector3.zero;
        }
        else
        {
            isGrabing = false;
            _objectToGrab.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}

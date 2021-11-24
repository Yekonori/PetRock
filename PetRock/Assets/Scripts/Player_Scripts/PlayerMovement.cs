using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Script Parameters
    [Header("..")]
    [SerializeField, Min(0f)] float speed = 5f;
    [SerializeField, Min(0f)] float rotationSpeed = 90f;
    [SerializeField, Min(0f)] float gravity = 5f;

    [SerializeField] bool debugMode = false;
    #endregion Script Parameters

    #region Fields

    // direction and rotation
    private float dirX = 0;
    private float dirY = 0;
    private Camera cam;
    private CharacterController characterController;
    private bool inDialog = false;
    private bool isMoving;

    private Animator anim;
    #endregion Fields

    private void Start()
    {
        cam = Camera.main;
        characterController = GetComponent<CharacterController>();
        inDialog = false;
        anim = GetComponent<Animator>();
        isMoving = false;
    }

    void Update()
    {
        if (GameManager.instance._inPause || GameManager.instance.inRockBalancing || PlayerParameters.Instance.IsOnTimeOut() || inDialog)
        {
            dirX = 0;
            dirY = 0;
        }

        Vector3 dir = new Vector3(dirX, 0, dirY);
        Vector3 moveDir = Vector3.zero;
        if (dir.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up), Vector3.up) * Quaternion.LookRotation(dir.normalized, Vector3.up);
            Quaternion rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

            transform.rotation = rotation;
            float magnitude = Mathf.Clamp01(dir.sqrMagnitude);
            if (!inDialog)
            {
                moveDir = magnitude * (rot * Vector3.forward).normalized * speed;
            }
            else
            {
                moveDir = Vector3.zero;
                anim.SetBool("talking", true);
            }
        }
        
        if(!inDialog && moveDir == Vector3.zero)
        {
            anim.SetBool("talking", false);
        }

        
        if (!isMoving && moveDir != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
            isMoving = true;
        }
        else if (isMoving && moveDir == Vector3.zero)
        {
            anim.SetBool("isMoving", false);
            isMoving = false;
        }
        
        float g = characterController.isGrounded ? 0.1f : gravity;
        characterController.Move((moveDir + g * Vector3.down) * Time.deltaTime);

        
    }

    public void SetMovementDirection(float x, float y)
    {
        dirX = x;
        dirY = y;
    }

    public void SetInDialog(bool value)
    {
        inDialog = value;
    }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;

        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}

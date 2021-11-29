using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial_Script : MonoBehaviour
{
    private bool _firstMovementPlayer = false;
    private bool _firstRockBalancing = false;

    bool _canTuto = false;

    bool _moveLeft = false;
    bool _moveRight = false;

    private void OnEnable()
    {
        GetComponent<CanvasGroup>().DOFade(1, 1);
        _canTuto = true;
    }

    public void IsFirstPlayerMovement()
    {
        _firstRockBalancing = false;
        _firstMovementPlayer = true;
    }

    public void IsFirstRockBalancing()
    {
        _firstMovementPlayer = false;
        _firstRockBalancing = true;
    }

    private void Update()
    {
        CheckMove();

        if(_canTuto && _moveLeft && _moveRight)
        {
            _canTuto = false;
            _moveLeft = false;
            _moveRight = false;

            Debug.LogError("tuto validate");

            GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => gameObject.SetActive(false));
        }
    }

    void CheckMove()
    {
        if (MoveLeftJoystick())
            _moveLeft = true;

        if (MoveRightJoystock())
            _moveRight = true;
    }

    bool MoveLeftJoystick()
    {
        return (GameManager.instance.player.GetAxis("DirX") > 0.01f || GameManager.instance.player.GetAxis("DirY") > 0.01f);
    }

    bool MoveRightJoystock()
    {
        return GameManager.instance.player.GetAxis("MoveFreeFollowCameraAround") > 0.01f;
    }
}

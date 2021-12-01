using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Tutorial_Script : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _leftText;
    [SerializeField]
    private TextMeshProUGUI _rightText;

    private const string _leftFirstMovementPlayerText = "Player movement";
    private const string _rightFirstMovementPlayerText = "Camera movement";

    private const string _leftFirstRockBalancingText = "Rock transalation";
    private const string _rightFirstRockBalancingText = "Rock rotation";

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
        ChangeTextTuto(_leftFirstMovementPlayerText, _rightFirstMovementPlayerText);
    }

    public void IsFirstRockBalancing()
    {
        ChangeTextTuto(_leftFirstRockBalancingText, _rightFirstRockBalancingText);
    }

    void ChangeTextTuto(string leftText, string rightText)
    {
        _leftText.text = leftText;
        _rightText.text = rightText;
    }

    private void Update()
    {
        CheckMove();

        if(_canTuto && _moveLeft && _moveRight)
        {
            _canTuto = false;
            _moveLeft = false;
            _moveRight = false;

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
        return (GameManager.instance.player.GetAxis("DirX") != 0.0f || GameManager.instance.player.GetAxis("DirY") != 0.0f);
    }

    bool MoveRightJoystock()
    {
        return GameManager.instance.player.GetAxis("MoveFreeFollowCameraAround") != 0.0f;
    }
}

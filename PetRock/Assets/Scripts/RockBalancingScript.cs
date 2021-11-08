using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class RockBalancingScript : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    [Range(0f, 0.5f)] float _deadZone = 0.01f;

    // direction and rotation
    private int _moveForward = 0;
    private int _moveRotation = 0;

    //Speed rock
    [Header("Speed rock movement")]
    [SerializeField, Min(0f)] float _speed = 5f;
    [SerializeField, Min(0f)] float _rotationSpeed = 90f;

    [Header("Start rock position")]
    [SerializeField]
    private Transform _startPosRock;

    [Header("Validate position rock")]
    [SerializeField]
    private Transform _finalPosRock;
    private Transform _finalA;
    private Transform _finalB;

    [Header("RockPrefab")]
    [SerializeField]
    private GameObject _prefabRock;
    private GameObject _theRock;

    private bool _canRockBalance = false;
    private bool _goodPlace = false;
    private bool _goodRot = false;

    [Header("Camera views")]
    [SerializeField]
    private TriggeredViewVolume _triggeredViewVolume;
    [SerializeField]
    private DollyViewAutoCircle _dollyView;

    private void OnEnable()
    {
        player = GameManager.instance.player;
        _theRock = Instantiate(_prefabRock, _startPosRock.position, Quaternion.identity, _startPosRock.parent);

        _finalA = _finalPosRock.GetChild(0);
        _finalB = _finalPosRock.GetChild(1);

        StartCoroutine(startRockBalance());
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canRockBalance)
            return;

        float move = player.GetAxis("MovePetRock");
        int moveInt = Mathf.Abs(move) > _deadZone ? (int)Mathf.Sign(move) : 0;

        float rotate = player.GetAxis("TiltPetRock");
        int rotateInt = Mathf.Abs(rotate) > _deadZone ? (int)Mathf.Sign(rotate) : 0;

        SetMovementDirection(moveInt, rotateInt);

        _theRock.transform.position += _moveForward * transform.forward * _speed * Time.deltaTime; // += gravity * 
        _theRock.transform.Rotate(Vector3.up, _moveRotation * _rotationSpeed * Time.deltaTime);

        if (ValidatePos() && player.GetButton("ValidatePetRockPos"))
        {
            EndRockBalancing();
        }
    }

    private void SetMovementDirection(float vertical, float rotate)
    {
        _moveForward = (int)vertical;
        _moveRotation = (int)rotate;
    }

    private bool ValidatePos()
    {
        if (_theRock.transform.position.z <= _finalA.position.z && _theRock.transform.position.z >= _finalB.position.z)
            _goodPlace = true;

        if (_theRock.transform.rotation == _finalPosRock.rotation)
            _goodRot = true;

        return _goodPlace && _goodRot;
    }

    private IEnumerator startRockBalance()
    {
        yield return new WaitForSeconds(1);
        _canRockBalance = true;
    }

    private void EndRockBalancing()
    {
        _triggeredViewVolume.view = _dollyView;
        _triggeredViewVolume.ActiveView(true);
        StartCoroutine(finishRockBalancing());
    }

    private IEnumerator finishRockBalancing()
    {
        yield return new WaitForSeconds(_dollyView.getTimeToRotate + 1);
        GetComponent<Collider>().enabled = false;
        _triggeredViewVolume.ActiveView(false);
        enabled = false;
        GameManager.instance.inRockBalancing = false;
    }
}

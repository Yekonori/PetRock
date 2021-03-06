using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using TMPro;

public class RockBalancingScript : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    [Range(0f, 0.5f)] public float deadZone = 0.01f;

    //Speed rock
    [Header("Speed rock movement")]
    [SerializeField, Min(0f)] float _speed = 5f;
    [SerializeField, Min(0f)] float _rotationSpeed = 90f;

    [Header("Start position")]
    [SerializeField]
    private Transform _startPosRock;
    public Transform startPlayerPos;

    [Header("First rock balancing"), SerializeField]
    private bool _isFirstRockBalancing = false;
    [ShowIf("@_isFirstRockBalancing == true"), SerializeField]
    private GameObject _tutoCanvas;

    [Header("Validate position rock")]
    [SerializeField]
    private Transform _finalPosRock;
    private Transform _finalA;
    private Transform _finalB;
    [SerializeField]
    private  float _margeRot;
    [SerializeField]
    private float _endZoomCamera;
    [SerializeField]
    private float _multiplicatorSpeedZoom;
    [SerializeField]
    private TextMeshProUGUI _textToContinue;
    [SerializeField]
    private TextMeshProUGUI _textToValidateRb;
    [SerializeField]
    private bool _loadNewScene = false;
    [ShowIf("@_loadNewScene == true"), SerializeField]
    private string _nextScene;
    [SerializeField]
    private float _timerDisplayText = 0.0f;


    private float _posVib;
    private float _rotVib;

    [Header("RockPrefab")]
    [SerializeField]
    private GameObject _prefabRock;
    private GameObject _theRock;

    private bool _canRockBalance = false;
    private bool _goodPlace = false;
    private bool _goodRot = false;
    private bool _endRockBalancing = false;
    private bool _onceEnd = false;

    [Header("Camera views")]
    [SerializeField]
    private TriggeredViewVolume _triggeredViewVolume;
    [SerializeField]
    private DollyViewAutoCircle _dollyView;
    [SerializeField]
    private FixedView _fixedView;

    private void OnEnable()
    {
        player = GameManager.instance.player;
        _theRock = Instantiate(_prefabRock, _startPosRock.position, Quaternion.identity, _startPosRock.parent);

        _finalA = _finalPosRock.GetChild(0);
        _finalB = _finalPosRock.GetChild(1);

        if(ControllerType.Instance != null)
        {
            if(ControllerType.Instance.typeController == ControllerType.TypeController.Playstation)
                _textToContinue.text = "Press ▲ to continue";
            else
                _textToContinue.text = "Press Y to continue";


            _textToValidateRb.text = "Stay press on any trigger to validate";
        }

        FirstRockBalancing();

        StartCoroutine(startRockBalance());
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canRockBalance || GameManager.instance._inPause)
            return;

        if (!_endRockBalancing)
        {
            float move = player.GetAxis("MovePetRock");
            move = Mathf.Abs(move) > deadZone ? move : 0;

            float rotate = player.GetAxis("TiltPetRock");
            rotate = Mathf.Abs(rotate) > deadZone ? rotate : 0;

            Vector3 newPos = _theRock.transform.position + move * -transform.forward * _speed * Time.deltaTime;
            newPos.z = Mathf.Clamp(newPos.z, -1, 1);

            _theRock.transform.position = newPos;
            _theRock.transform.Rotate(Vector3.right, -rotate * _rotationSpeed * Time.deltaTime);

            _posVib = Mathf.Clamp(Mathf.Abs(_theRock.transform.position.z - _finalPosRock.position.z), 0f, 0.5f);
            _rotVib = Mathf.Clamp(Mathf.Abs((_theRock.transform.rotation.x - _finalPosRock.rotation.x)), 0f, 0.5f);

        
            if(ValidateRot() && ValidatePos())
            {
                _textToValidateRb.DOFade(1, 1);
                if (player.GetButton("ValidatePetRockPos"))
                {
                    if (_fixedView.fov >= _endZoomCamera)
                        _fixedView.fov -= Time.deltaTime * (_multiplicatorSpeedZoom * 10);
                    else
                    {
                        DOTween.To(() => _fixedView.fov, x => _fixedView.fov = x, 75, 0.5f).SetDelay(1).OnComplete(() =>
                        {
                            _endRockBalancing = true;
                            _textToValidateRb.DOFade(0, 1);
                        });
                    }
                }
                else if (player.GetButtonUp("ValidatePetRockPos"))
                {
                    DOTween.To(() => _fixedView.fov, x => _fixedView.fov = x, 75, 1.5f);
                }
            }
            else
                _textToValidateRb.DOFade(0, 1);
        }

        if (_endRockBalancing)
        {
            player.SetVibration(0, 0);
            player.StopVibration();
            EndRockBalancing();
        }
        else
        {
            if(!_endRockBalancing && !GameManager.instance._inPause)
                player.SetVibration(0, _posVib + _rotVib);
        }
    }

    private void FirstRockBalancing()
    {
        if (_isFirstRockBalancing)
        {
            _tutoCanvas.SetActive(true);
            _tutoCanvas.GetComponent<Tutorial_Script>().IsFirstRockBalancing();
        }
    }

    private bool ValidatePos()
    {
        if (_theRock.transform.position.z <= _finalA.position.z 
            && _theRock.transform.position.z >= _finalB.position.z)
            _goodPlace = true;
        else
            _goodPlace = false;

        return _goodPlace;
    }

    private bool ValidateRot()
    {
        if (_theRock.transform.localRotation.eulerAngles.x >= (_finalPosRock.localRotation.eulerAngles.x - _margeRot)
            && _theRock.transform.localRotation.eulerAngles.x <= (_finalPosRock.localRotation.eulerAngles.x + _margeRot))
            _goodRot = true;
        else
            _goodRot = false;

        return _goodRot;
    }

    private IEnumerator startRockBalance()
    {
        yield return new WaitForSeconds(1);
        _canRockBalance = true;
    }

    private void EndRockBalancing()
    {
        if (!_onceEnd)
        {
            StartCoroutine(finishRockBalancing());
            _onceEnd = true;
        }
    }

    private IEnumerator finishRockBalancing()
    {
        _theRock.GetComponentInChildren<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(1);

        _dollyView.SetActive(true);

        GetComponent<Collider>().enabled = false;

        GetComponent<RockBalancing_Dialogue>().EndDialogue();
        yield return new WaitWhile(() => GetComponent<RockBalancing_Dialogue>().CheckEndDialogue());

        _textToContinue.DOFade(1, 1).SetDelay(_timerDisplayText);

        if (_loadNewScene)
        {
            yield return new WaitUntil(() => player.GetButton("ValidateEndRockBalancing"));

            GameManager.instance.TransitionCanvas(1).OnComplete(() =>
            {
                GameManager.instance.TransitionTextDialogue(1).OnComplete(() =>
                {
                    GameManager.instance.TransitionTextDialogue(0).SetDelay(5).OnComplete(() =>
                    {
                        SceneManager.LoadScene(_nextScene);
                    });
                });
            });
        }
        else
        {
            _dollyView.SetActive(false);
            _triggeredViewVolume.ActiveView(false);

            Destroy(_theRock);

            enabled = false;
            GameManager.instance.inRockBalancing = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public enum MovementType
{
    NoMovement,
    Moving
}

public enum RotationType
{
    NoRotation,
    EssuieGlace,
    FullRotation
}

[Serializable]
public class EyeParam
{
    [Header("Movement")]
    public MovementType movementType;
    public Transform startPos;
    [ShowIf("movementType", MovementType.Moving)] public Transform endPos;
    [ShowIf("movementType", MovementType.Moving)] public float timeToMove = 5;

    [Header("Rotation")]
    public RotationType rotationType;
    [ShowIf("rotationType", RotationType.FullRotation)] public float rotationSpeed;
    [ShowIf("rotationType", RotationType.EssuieGlace)] public float maxAngle;
    [ShowIf("rotationType", RotationType.EssuieGlace)] public float timeToRotate;
}

[Serializable]
public class EyeInfo
{
    public GameObject Eye;
    public float timeAlive;
    [Header("FieldOfView")]
    [Min(0)] public float range = 5;
    [Range(0, 360)] public float viewAngle = 45;
    [Space]
    public EyeParam eyeParam;
}

public class EyeManager : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] List<EyeInfo> eyeInfos;

    private EyeInfo currentEyeInfo;
    private float timer = 0f;
    private int currentEyeInfoIndex;
    private int nbEyeInfo;

    private void Start()
    {
        if (eyeInfos == null || eyeInfos.Count == 0)
        {
            Debug.LogError("No Eye Infos");
            Application.Quit();
        }
        if (CheckParam())
        {
            Application.Quit();
        }

        nbEyeInfo = eyeInfos.Count;
        currentEyeInfoIndex = 0;
        currentEyeInfo = eyeInfos[currentEyeInfoIndex];

        InitEyes();
        currentEyeInfo.Eye.SetActive(true);
    }

    private void Update()
    {
        if (nbEyeInfo == 1) return;

        timer += Time.deltaTime;
        if (timer > currentEyeInfo.timeAlive)
        {
            currentEyeInfo.Eye.SetActive(false);

            if (currentEyeInfoIndex == nbEyeInfo - 1)
                currentEyeInfoIndex = 0;
            else currentEyeInfoIndex++;

            currentEyeInfo = eyeInfos[currentEyeInfoIndex];
            currentEyeInfo.Eye.SetActive(true);

            timer = 0;
        }
    }

    private bool CheckParam()
    {
        bool hasError = false;
        for (int i = 0; i < eyeInfos.Count; ++i)
        {
            if (eyeInfos[i].eyeParam == null)
            {
                hasError = true;
                Debug.LogError($"Param is empty for eye {i+1}");
            }
            else
            {
                EyeParam param = eyeInfos[i].eyeParam;
                if (!param.startPos)
                {
                    hasError = true;
                    Debug.LogError($"No start position for eye {i + 1}");
                }

                if (param.movementType == MovementType.Moving && !param.endPos)
                {
                    hasError = true;
                    Debug.LogError($"No end position for eye {i + 1}");
                }

                switch (param.rotationType)
                {
                    case RotationType.NoRotation:
                        break;
                    case RotationType.EssuieGlace:
                        if (param.maxAngle == 0 || param.timeToRotate == 0)
                        {
                            hasError = true;
                            Debug.LogError($"No angle or timeToRotate for eye {i+1}"); 
                        }
                        break;
                    case RotationType.FullRotation:
                        if (param.rotationSpeed == 0)
                        {
                            hasError = true;
                            Debug.LogError($"No rotation speed for eye {i+1}");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        return hasError;
    }

    private void InitEyes()
    {
        foreach (EyeInfo info in eyeInfos)
        {
            info.Eye.SetActive(false);
            info.Eye.GetComponent<EyeMovement>().Init(info.eyeParam, animationCurve);
            info.Eye.GetComponent<FieldOfView>().Init(info.range, info.viewAngle);
        }
    }
}
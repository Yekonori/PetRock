using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speed = 3f;

    private Camera myCam;
    private CameraConfiguration currentConfig;
    private CameraConfiguration targetConfig;

    private List<AView> activeViews = new List<AView>();
    private bool isCutRequested = false;

    public static CameraController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        myCam = Camera.main;
        //StartCoroutine(MoveConfig(listConfig));
    }

    private void Update()
    {
        targetConfig = InterpolateView(activeViews);
        currentConfig = GetCurrentConfig();

        Quaternion orientation = Quaternion.Euler(targetConfig.pitch, targetConfig.yaw, targetConfig.roll);
        Vector3 offset = orientation * (Vector3.back * targetConfig.distance);

        float s = speed * Time.deltaTime;

        if (isCutRequested)
        {
            myCam.transform.rotation = orientation;
            myCam.transform.position = targetConfig.pivot + offset;
            myCam.fieldOfView = targetConfig.fieldOfView;

            isCutRequested = false;
        }
        else
        {
            if (s < 1f)
            {
                myCam.transform.rotation = Quaternion.Lerp(currentConfig.GetRotation(), targetConfig.GetRotation(), s);
                myCam.transform.position = Vector3.Lerp(currentConfig.GetPosition(), targetConfig.GetPosition(), s);
                myCam.fieldOfView = Mathf.Lerp(currentConfig.fieldOfView, targetConfig.fieldOfView, s);
            }
            else
            {
                myCam.transform.rotation = orientation;
                myCam.transform.position = targetConfig.pivot + offset;
                myCam.fieldOfView = targetConfig.fieldOfView;
            }
        }
    }

    public void AddView(AView view)
    {
        activeViews.Add(view);
    }

    public void RemoveView(AView view)
    {
        activeViews.Remove(view);
    }

    CameraConfiguration GetCurrentConfig()
    {
        float yaw = myCam.transform.rotation.eulerAngles.y;
        float pitch = myCam.transform.rotation.eulerAngles.x;
        float roll = myCam.transform.rotation.eulerAngles.z;
        Vector3 pivot = myCam.transform.position;
        float distance = 0f;
        float fieldOfView = myCam.fieldOfView;

        return new CameraConfiguration(yaw, pitch, roll, pivot, distance, fieldOfView);
    }
    CameraConfiguration InterpolateView(List<AView> activeViews)
    {
        if (activeViews.Count == 0)
        {
            Debug.LogError("Ajoute des vues !");
            return new CameraConfiguration(0f, 0f, 0f, Vector3.zero, 5f, 60f);
        }

        float yaw = 0f;
        float pitch = 0f;
        float roll = 0f;
        Vector3 pivot = Vector3.zero;
        float distance = 0f;
        float fieldOfView = 0f;

        float sumWeight = 0f;

        Vector2 sum = Vector2.zero;

        foreach (AView view in activeViews)
        {
            //yaw += view.weight * view.GetConfiguration().yaw;
            sum += new Vector2(Mathf.Cos(view.GetConfiguration().yaw * Mathf.Deg2Rad), Mathf.Sin(view.GetConfiguration().yaw * Mathf.Deg2Rad)) * view.weight;

            pitch += view.weight * view.GetConfiguration().pitch;
            roll += view.weight * view.GetConfiguration().roll;
            pivot += view.weight * view.GetConfiguration().pivot;
            distance += view.weight * view.GetConfiguration().distance;
            fieldOfView += view.weight * view.GetConfiguration().fieldOfView;

            sumWeight += view.weight;
        }
        if (sumWeight == 0)
        {
            Debug.LogError("Met des poids à tes vues");
            sum = Vector2.zero;

            foreach (AView view in activeViews)
            {
                //yaw += view.GetConfiguration().yaw;
                sum += new Vector2(Mathf.Cos(view.GetConfiguration().yaw * Mathf.Deg2Rad), Mathf.Sin(view.GetConfiguration().yaw * Mathf.Deg2Rad)) * 1 / activeViews.Count;

                pitch += view.GetConfiguration().pitch;
                roll += view.GetConfiguration().roll;
                pivot += view.GetConfiguration().pivot;
                distance += view.GetConfiguration().distance;
                fieldOfView += view.GetConfiguration().fieldOfView;
            }
            yaw = Vector2.SignedAngle(Vector2.right, sum);
            float nbView = activeViews.Count;
            return new CameraConfiguration(yaw, pitch / nbView, roll / nbView, pivot / nbView, distance / nbView, fieldOfView/ nbView);
        }
        //yaw /= sumWeight;
        yaw = Vector2.SignedAngle(Vector2.right, sum);

        pitch /= sumWeight;
        roll /= sumWeight;
        pivot /= sumWeight;
        distance /= sumWeight;
        fieldOfView /= sumWeight;

        return new CameraConfiguration(yaw, pitch, roll, pivot, distance, fieldOfView);
    }

    public float ComputeAverageYaw()
    {
        Vector2 sum = Vector2.zero;
        foreach (AView view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            sum += new Vector2(Mathf.Cos(config.yaw * Mathf.Deg2Rad), Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * view.weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
    }

    public void Cut()
    {
        isCutRequested = true;
    }



    // My test
    [SerializeField] float movementDuration = 5f;
    public List<CameraConfiguration> listConfig;

    IEnumerator MoveConfig(List<CameraConfiguration> lConfig)
    {
        if (lConfig.Count == 0)
        {
            yield return null;
        }
        else if (lConfig.Count == 1)
        {
            Quaternion orientation = Quaternion.Euler(lConfig[0].pitch, lConfig[0].yaw, lConfig[0].roll);
            myCam.transform.rotation = orientation;
            Vector3 offset = orientation * (Vector3.back * lConfig[0].distance);
            transform.position = lConfig[0].pivot + offset;
            myCam.fieldOfView = lConfig[0].fieldOfView;
        }
        else
        {
            float timer = 0f;
            while (timer < movementDuration)
            {
                timer += Time.deltaTime;

                CameraConfiguration bla = CameraConfiguration.ListInterpolation(timer / movementDuration, lConfig);

                Quaternion orientation = Quaternion.Euler(bla.pitch, bla.yaw, bla.roll);
                myCam.transform.rotation = orientation;

                Vector3 offset = orientation * Vector3.back * bla.distance;
                myCam.transform.position = bla.pivot + offset;
                myCam.fieldOfView = bla.fieldOfView;

                yield return null;
            }
        }
    }
}

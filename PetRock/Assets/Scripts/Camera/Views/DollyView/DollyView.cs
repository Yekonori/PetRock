using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
{
    public float roll = 0f;
    public float distance = 0f;
    public float fov = 0f;

    [SerializeField] Transform target;
    [SerializeField] Rail rail;
    [SerializeField] float speed;

    public bool isAuto = false;

    private Vector3 railPosition = Vector3.zero;

    private float yaw = 0f;
    private float pitch = 0f;

    private float dist = 0f;

    private Vector3 debugClosestPoint;

    private void Update()
    {
        if (!isAuto)
        {
            if (isActive)
            {
                float inputMouseWheel = Input.GetAxis("Mouse ScrollWheel");
                dist += speed * Time.deltaTime * inputMouseWheel;
            }
            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    dist += speed * Time.deltaTime;
            //}
            //else if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    dist -= speed * Time.deltaTime;
            //}
            railPosition = rail.GetPosition(dist);
        }
        else
        {
            Transform closestNode = rail.nodes[0];
            float minDistance = Mathf.Infinity;
            int closestNodeNumber = 0;
            
            for (int i = 0; i < rail.nodes.Count; ++i)
            {
                float currentDistance = (target.position - rail.nodes[i].position).sqrMagnitude;
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closestNode = rail.nodes[i];
                    closestNodeNumber = i;
                }
            }

            float d1 = Mathf.Infinity;
            Vector3 pos1 = Vector3.zero;
            float d2 = Mathf.Infinity;
            Vector3 pos2 = Vector3.zero;

            Vector3 toPrevious = Vector3.zero;
            Vector3 toNext = Vector3.zero;

            if (closestNodeNumber == 0)
            {
                toPrevious = rail.nodes[rail.nodes.Count - 1].position - rail.nodes[closestNodeNumber].position;
                toNext = rail.nodes[closestNodeNumber + 1].position - rail.nodes[closestNodeNumber].position;
            }
            else if (closestNodeNumber == rail.nodes.Count - 1)
            {
                toPrevious = rail.nodes[closestNodeNumber - 1].position - rail.nodes[closestNodeNumber].position;
                toNext = rail.nodes[0].position - rail.nodes[closestNodeNumber].position;
            }
            else
            {
                toPrevious = rail.nodes[closestNodeNumber - 1].position - rail.nodes[closestNodeNumber].position;
                toNext = rail.nodes[closestNodeNumber + 1].position - rail.nodes[closestNodeNumber].position;
            }

            pos1 = closestNode.position + Mathf.Clamp(Vector3.Dot(target.position - closestNode.position, toPrevious.normalized), 0f, toPrevious.magnitude) * toPrevious.normalized;
            d1 = (target.position - pos1).sqrMagnitude;

            pos2 = closestNode.position + Mathf.Clamp(Vector3.Dot(target.position - closestNode.position, toNext.normalized), 0f, toNext.magnitude) * toNext.normalized;
            d2 = (target.position - pos2).sqrMagnitude;

            if (!rail.isLoop)
            {
                if (closestNodeNumber == 0)
                {
                    railPosition = pos2;
                }
                else if (closestNodeNumber == rail.nodes.Count - 1)
                {
                    railPosition = pos1;
                }
                else
                {
                    if (d1 < d2)
                    {
                        railPosition = pos1;
                    }
                    else
                    {
                        railPosition = pos2;
                    }
                }
            }
            else
            {
                if (d1 < d2)
                {
                    railPosition = pos1;
                }
                else railPosition = pos2;
            }
        }

        Quaternion rot = Quaternion.LookRotation(target.position - railPosition);
        yaw = rot.eulerAngles.y;
        pitch = rot.eulerAngles.x;
    }

    public override CameraConfiguration GetConfiguration()
    {
        return new CameraConfiguration(yaw, pitch, roll, railPosition, distance, fov);
    }
}

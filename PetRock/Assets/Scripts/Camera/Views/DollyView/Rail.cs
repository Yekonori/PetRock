using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool isLoop = false;
    public List<Transform> nodes;// = new List<Transform>();
    private float length = 0f;
    private List<float> nodesDistance = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        if (nodes.Count > 1)
        {
            for (int i = 0; i < nodes.Count - 1; ++i)
            {
                float currentLength = (nodes[i].position - nodes[i + 1].position).magnitude;
                nodesDistance.Add(currentLength);
                length += currentLength;
            }
            if (isLoop)
            {
                float currentLength = (nodes[nodes.Count - 1].position - nodes[0].position).magnitude;
                nodesDistance.Add(currentLength);
                length += currentLength;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (nodes.Count > 1)
        {
            for (int i = 0; i < nodes.Count - 1; ++i)
            {
                Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
            }
            if (isLoop)
            {
                Gizmos.DrawLine(nodes[nodes.Count - 1].position, nodes[0].position);
            }
        }
    }

    public float GetLength()
    {
        return length;
    }

    public Vector3 GetPosition(float distance)
    {
        if (nodes.Count == 0)
            return Vector3.zero;

        if (nodes.Count == 1)
            return nodes[0].position;

        if (isLoop)
        {
            distance = distance % length;
        }
        else
        {
            if (distance < 0f)
            {
                distance = 0f;
            }
            else if (distance > length)
            {
                distance = length;
            }
        }

        for (int i = 0; i < nodes.Count - 1; ++i)
        {
            if (distance < nodesDistance[i])
            {
                return Vector3.Lerp(nodes[i].position, nodes[i + 1].position, distance / nodesDistance[i]);
            }
            else
            {
                distance -= nodesDistance[i];
            }
        }
        return Vector3.Lerp(nodes[nodes.Count - 1].position, nodes[0].position, distance / nodesDistance[nodesDistance.Count - 1]);
    }

    public List<float> GetNodesDistance()
    {
        return nodesDistance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShaping : MonoBehaviour
{
    public ShapingType shapeType;
    public float radius = 3f;
    public float m_childSpace = 1f;
    public Vector3[] localNodes = new Vector3[1];
    public float pathLength;

    public void UpdateCirlePosition()
    {
        if(transform.childCount == 0)
            return;
        float ratio = 360f / transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = new Vector3(Mathf.Cos(Mathf.PI * ratio * i / 180) * radius, Mathf.Sin(Mathf.PI * ratio * i / 180) * radius, 0);
        }
    }
    public void UpdatePolygonPosition()
    {
        if(localNodes.Length < 1)
            return;
        int childCount = transform.childCount;
        float islandPosOffset = 0;
        List<float> distances = new List<float>();
        pathLength = 0;
        for(int i = 1; i < localNodes.Length; i++)
        {
            float distance = Vector3.Distance(transform.TransformPoint(localNodes[i]), transform.TransformPoint(localNodes[i - 1]));
            distances.Add(distance);
            pathLength += distance;
        }
        float distanceBetweenIslands = pathLength / childCount;
        islandPosOffset = childCount == 1 ? (pathLength / 2) : ((pathLength - ((childCount - 1) * distanceBetweenIslands)) / 2);

        for(int i = 0; i < childCount; i++)
        {
            int childIndex = i;
            int destinationLineIndex = 0;
            float tempDistance = 0;
            float destinationDistance = distanceBetweenIslands * (childIndex + 1) - islandPosOffset;

            Vector3 direction = Vector3.zero;
            float distanceFromStartPos = 0;
            Vector3 startPosByLineIndex = Vector3.zero;
            
            for(int j = 0; j < distances.Count; j++)
            {
                tempDistance += distances[j];
                if(tempDistance >= destinationDistance)
                {
                    destinationLineIndex = j;
                    int startPosIndex = destinationLineIndex;
                    int endPosIndex = startPosIndex + 1;
                    distanceFromStartPos = destinationDistance - (tempDistance - distances[destinationLineIndex]);
                    startPosByLineIndex = transform.TransformPoint(localNodes[startPosIndex]);
                    direction = (transform.TransformPoint(localNodes[endPosIndex]) - startPosByLineIndex).normalized;
                    break;
                }
            }
            transform.GetChild(childIndex).transform.position = startPosByLineIndex + direction * distanceFromStartPos;
        }
    }

    public void UpdatePathLength()
    {
        pathLength = 0;
        for(int i = 1; i < localNodes.Length; i++)
        {
            float distance = Vector3.Distance(transform.TransformPoint(localNodes[i]), transform.TransformPoint(localNodes[i - 1]));
            pathLength += distance;
        }
    }
}
public enum ShapingType
{
    Circle,
    Polygon
}

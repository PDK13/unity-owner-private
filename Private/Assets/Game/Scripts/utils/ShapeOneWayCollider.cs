using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ShapeOneWayCollider : MonoBehaviour
{
    [SerializeField] private Vector2 m_offset = Vector2.zero;
    void Start ()
    {
        SpriteShapeController ssc = GetComponent<SpriteShapeController>();
        Spline spl = ssc.spline;
        List<List<int>> paths = GetPaths(spl);

        for (int i = 0; i < paths.Count; i++)
        {
            List<int> path = paths[i];
            List<Vector2> realPath = new List<Vector2>();
            for (int j = 0; j < path.Count; j++)
            {
                realPath.Add(spl.GetPosition(path[j]));
            }
            EdgeCollider2D collider = gameObject.AddComponent<EdgeCollider2D>();
            collider.usedByEffector = true;
            collider.points = realPath.ToArray();
        }
    }

    List<List<int>> GetPaths(Spline spl)
    {
        int max = spl.GetPointCount();
        List<List<int>> paths = new List<List<int>>();
        List<int> currentPath = new List<int>();
        for (int i = 0; i < max; i++)
        {
            Vector2 pos1 = spl.GetPosition(i);
            int next = i == (max - 1) ? 0 : (i + 1);
            Vector2 pos2 = spl.GetPosition(next);
            Vector2 right = (pos2 - pos1).normalized;
            float angleLine = Mathf.Abs(Vector2.Angle(right, Vector2.right));
            Vector2 pos3 = spl.GetPosition(i == 0 ? (max - 1) : (i - 1));
            Vector2 left = (pos3 - pos1).normalized;
            
            float a = MirrorAngle(Vector2.up, left);
            float b = MirrorAngle(left, right);
            float c = a + (b * 0.5f);
            if (b > 0)
                c = (180 + c);
            c = Quaternion.Euler(0, 0, c).eulerAngles.z;
            
            bool validLine = (angleLine >= 0 && angleLine < 20) || (180 - angleLine) < 20;
            bool validAngle = c < 90 && c > 0;
            if (validLine && validAngle)
            {
                if(!currentPath.Contains(i))
                    currentPath.Add(i);
                if(!currentPath.Contains(next))
                    currentPath.Add(next);
            }
            else
            {
                if (currentPath.Count > 1 && !currentPath.Contains(i))
                {
                    paths.Add(currentPath);
                    currentPath = new List<int>();
                }
                else
                    currentPath = new List<int>();
            }
            if (currentPath.Count > 1)
                paths.Add(currentPath);
        }

        return paths;
    }
    
    float MirrorAngle(Vector2 a, Vector2 b)
    {
        float dot = Vector2.Dot(a, b);
        float det = (a.x * b.y) - (b.x * a.y);
        return Mathf.Atan2(det, dot) * Mathf.Rad2Deg;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        SpriteShapeController ssc = GetComponent<SpriteShapeController>();
        Spline spl = ssc.spline;
        List<List<int>> paths = GetPaths(spl);
        Vector2 offset = (Vector2)transform.localPosition + m_offset;
        
        for (int i = 0; i < paths.Count; i++)
        {
            List<int> path = paths[i];
            Vector2 start = (Vector2)spl.GetPosition(path[0]) + offset;
            for (int j = 1; j < path.Count; j++)
            {
                Vector2 next = (Vector2)spl.GetPosition(path[j]) + offset;
                Gizmos.DrawLine(start, next);
                start = next;
            }
        }
    }
}

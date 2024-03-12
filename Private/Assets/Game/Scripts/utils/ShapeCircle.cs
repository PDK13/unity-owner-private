using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class ShapeCircle : MonoBehaviour
{
    [Min(2f)]
    [SerializeField] private float m_radius = 5;
    [Min(1)]
    [SerializeField] private int m_piceCut = 1;

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
            UpdateCirCle();
    }
#endif

    void UpdateCirCle()
    {
        SpriteShapeController shape = GetComponent<SpriteShapeController>();
        Spline spl = shape.spline;

        float piceAngle = 2 / m_radius * 60;
        int count = (int)(360 / piceAngle);
        int max = spl.isOpenEnded ?  (count - m_piceCut + 1) : count;
        piceAngle = 360f / count;
        spl.Clear();
        List<Vector3> path = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            float angle = -i * piceAngle * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * m_radius;
            float y = Mathf.Sin(angle) * m_radius;
            Vector3 pos = new Vector3(x, y);
            path.Add(pos);
            if(i < max)
                spl.InsertPointAt(i, new Vector3(x, y));
        }

        max = spl.GetPointCount();
        for (int i = 0; i < max; i++)
        {
            spl.SetTangentMode(i, ShapeTangentMode.Continuous);
            int prevIndex = i == 0 ? (count - 1) : (i - 1);
            int nextIndex = i == (count - 1) ? 0 : (i + 1);
            Vector3 prevPos = path[prevIndex];
            Vector3 nextPos = path[nextIndex];
            SplineUtility.CalculateTangents(spl.GetPosition(i), prevPos, nextPos, transform.forward, 0.71f, out Vector3 rightTangent, out Vector3 leftTangent);
            spl.SetLeftTangent(i, leftTangent);
            spl.SetRightTangent(i, rightTangent);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class QShapeCircum
{
    #region Local

    private float m_radius = 0f;

    private bool m_hollow  = false;
    private float m_radiusHollow = 0f;
    private int m_cutHollow = 1;

    private Vector3[] m_points;

    private Spline m_spline;
    private SpriteShapeController m_spriteShapeController;
    private Transform m_spriteShapeControllerTransform;

    public QShapeCircum(SpriteShapeController SpriteShapeController)
    {
        m_spriteShapeController = SpriteShapeController;
        m_spline = SpriteShapeController.spline;
        m_spriteShapeControllerTransform = SpriteShapeController.transform;
    }

    public void SetFilledGenerate(float Radius)
    {
        m_radius = Radius;
        //
        float PiceAngle = 2 / m_radius * 60;
        int PointsCount = (int)(360 / PiceAngle);
        int SplineCount = m_spline.isOpenEnded ? (PointsCount - m_cutHollow + 1) : PointsCount;
        PiceAngle = 360f / PointsCount;
        //
        m_spline.Clear();
        m_points = new Vector3[PointsCount];
        for (int i = 0; i < PointsCount; i++)
        {
            float Angle = -i * PiceAngle * Mathf.Deg2Rad;
            Vector3 Pos = new Vector3(Mathf.Cos(Angle) * m_radius, Mathf.Sin(Angle) * m_radius, 0);
            //
            m_points[i] = Pos;
            //
            if (i < SplineCount)
                m_spline.InsertPointAt(i, Pos);
        }
        //
        SplineCount = m_spline.GetPointCount();
        for (int i = 0; i < SplineCount; i++)
        {
            m_spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            int PrevIndex = i == 0 ? (PointsCount - 1) : (i - 1);
            int NextIndex = i == (PointsCount - 1) ? 0 : (i + 1);
            Vector3 PrevPos = m_points[PrevIndex];
            Vector3 NextPos = m_points[NextIndex];
            SplineUtility.CalculateTangents(m_spline.GetPosition(i), PrevPos, NextPos, m_spriteShapeControllerTransform.forward, 0.71f, out Vector3 rightTangent, out Vector3 leftTangent);
            m_spline.SetLeftTangent(i, leftTangent);
            m_spline.SetRightTangent(i, rightTangent);
        }
    }

    public void SetHollowGenerate(float Radius, float RadiusHollow, int CutHollow)
    {
        m_radius = Radius;
        m_radiusHollow = RadiusHollow;
        m_cutHollow = CutHollow;
        //
        List<Vector3> Points = new List<Vector3>();
        //
        float OuterPiceAngle = 2 / Radius * 60;
        int OuterPointsCount = (int)(360 / OuterPiceAngle);
        int OuterSplineCount = m_spline.isOpenEnded ? (OuterPointsCount - m_cutHollow + 1) : OuterPointsCount;
        //int OuterSplineCount = OuterPointsCount - m_cutHollow + 1;
        OuterPiceAngle = 360f / OuterPointsCount;
        //
        m_spline.Clear();
        //
        for (int i = 0; i < OuterPointsCount; i++)
        {
            float Angle = -i * OuterPiceAngle * Mathf.Deg2Rad;
            Vector3 Pos = new Vector3(Mathf.Cos(Angle) * Radius, Mathf.Sin(Angle) * Radius, 0);
            //
            Points.Add(Pos);
            //
            if (i < OuterSplineCount)
                m_spline.InsertPointAt(i, Pos);
        }
        //
        float InnerPiceAngle = 2 / RadiusHollow * 60;
        int InnerPointsCount = (int)(360 / InnerPiceAngle);
        int InnerSplineCount = m_spline.isOpenEnded ? (InnerPointsCount - m_cutHollow + 1) : InnerPointsCount;
        //int InnerSplineCount = InnerPointsCount - m_cutHollow + 1;
        InnerPiceAngle = 360f / InnerPointsCount;
        //
        //
        for (int i = 0; i < InnerPointsCount; i++)
        {
            int IndexNew = InnerPointsCount - 1 - i;
            //
            float Angle = -IndexNew * InnerPiceAngle * Mathf.Deg2Rad;
            Vector3 Pos = new Vector3(Mathf.Cos(Angle) * RadiusHollow, Mathf.Sin(Angle) * RadiusHollow, 0);
            //
            Points.Add(Pos);
            //
            if (IndexNew < InnerSplineCount)
                m_spline.InsertPointAt(OuterPointsCount + i, Pos);
        }
        //
        m_points = Points.ToArray();
        //
        float SplineCount = m_spline.GetPointCount();
        int PointsCount = OuterPointsCount + InnerPointsCount;
        for (int i = 0; i < SplineCount; i++)
        {
            m_spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            int PrevIndex = i == 0 ? (PointsCount - 1) : (i - 1);
            int NextIndex = i == (PointsCount - 1) ? 0 : (i + 1);
            Vector3 PrevPos = m_points[PrevIndex];
            Vector3 NextPos = m_points[NextIndex];
            SplineUtility.CalculateTangents(m_spline.GetPosition(i), PrevPos, NextPos, m_spriteShapeControllerTransform.forward, 0.71f, out Vector3 rightTangent, out Vector3 leftTangent);
            m_spline.SetLeftTangent(i, leftTangent);
            m_spline.SetRightTangent(i, rightTangent);
        }
    }

    #endregion
}
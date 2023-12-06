using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class RendererCircumShape : MonoBehaviour
{
    [SerializeField] private SpriteShapeController m_spriteShape;

    [Space]
    [Min(2f)] public float FilledRadius = 2f;

    [Space]
    [Min(1)] public int FilledCut = 1;

    [Space]
    [Min(1f)] public float HollowRadius = 1f;
    [Min(0)] public int HollowCut = 0;

    [Space]
    public RendererCircumShapeData Circum;

    public void SetGenerateFilled()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new RendererCircumShapeData(m_spriteShape);
        //
        m_spriteShape.spline.isOpenEnded = false;
        Circum.SetFilledGenerate(FilledRadius);
    }

    public void SetGenerateFilledOpen()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new RendererCircumShapeData(m_spriteShape);
        //
        m_spriteShape.spline.isOpenEnded = true;
        Circum.SetFilledGenerate(FilledRadius, FilledCut);
    }

    public void SetGenerateHollow()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new RendererCircumShapeData(m_spriteShape);
        //
        m_spriteShape.spline.isOpenEnded = false;
        Circum.SetHollowGenerate(FilledRadius, HollowRadius, HollowCut);
    }
}

[Serializable]
public class RendererCircumShapeData
{
    private float m_radius = 0f;

    private bool m_hollow = false;
    private float m_radiusHollow = 0f;
    private int m_cutHollow = 1;

    private Vector3[] m_points;

    public float Radius => m_radius;

    public float RadiusHollow => m_hollow ? m_radiusHollow : 0f;

    public int CutHollow => m_cutHollow;

    public Vector3[] Points => m_points;

    private Spline m_spline;
    private SpriteShapeController m_spriteShapeController;
    private Transform m_spriteShapeControllerTransform;

    public RendererCircumShapeData(SpriteShapeController SpriteShapeController)
    {
        m_spriteShapeController = SpriteShapeController;
        m_spline = SpriteShapeController.spline;
        m_spriteShapeControllerTransform = SpriteShapeController.transform;
    }

    public void SetFilledGenerate(float Radius, int CutHollow = 0)
    {
        m_spline.Clear();
        //
        m_radius = Radius;
        m_hollow = false;
        m_radiusHollow = 0f;
        m_cutHollow = CutHollow;
        //
        List<Vector3> Points = new List<Vector3>();
        //
        int PointsCount = (int)(360 / (2 / m_radius * 60));
        int SplineCount = m_spline.isOpenEnded ? (PointsCount - CutHollow + 1) : PointsCount;
        float PiceAngle = 360f / PointsCount;
        //
        for (int i = 0; i < PointsCount; i++)
        {
            float Angle = -i * PiceAngle * Mathf.Deg2Rad;
            Vector3 Pos = new Vector3(Mathf.Cos(Angle) * m_radius, Mathf.Sin(Angle) * m_radius, 0);
            //
            Points.Add(Pos);
            //
            if (i < SplineCount)
                m_spline.InsertPointAt(i, Pos);
        }
        //
        m_points = Points.ToArray();
        //
        SetSplineGenerate();
    }

    public void SetHollowGenerate(float Radius, float RadiusHollow, int CutHollow = 0)
    {
        m_spline.Clear();
        //
        m_radius = Radius;
        m_hollow = true;
        m_radiusHollow = RadiusHollow;
        m_cutHollow = CutHollow;
        //
        List<Vector3> Points = new List<Vector3>();
        //
        int PointsCount = (int)(360 / (2 / m_radius * 60));
        int SplineCount = PointsCount;
        float PiceAngle = 360f / PointsCount;
        //
        for (int i = 0; i < PointsCount; i++)
        {
            if (i >= PointsCount - m_cutHollow)
                continue;
            //
            float Angle = -i * PiceAngle * Mathf.Deg2Rad;
            Vector3 Pos = new Vector3(Mathf.Cos(Angle) * m_radius, Mathf.Sin(Angle) * m_radius, 0);
            //
            Points.Add(Pos);
            //
            if (i < SplineCount)
                m_spline.InsertPointAt(i, Pos);
        }
        //
        for (int i = 0; i < PointsCount; i++)
        {
            if (i < m_cutHollow)
                continue;
            //
            int InerIndex = PointsCount - 1 - i;
            //
            float Angle = -InerIndex * PiceAngle * Mathf.Deg2Rad;
            Vector3 Pos = new Vector3(Mathf.Cos(Angle) * m_radiusHollow, Mathf.Sin(Angle) * m_radiusHollow, 0);
            //
            Points.Add(Pos);
            //
            if (InerIndex < SplineCount)
                m_spline.InsertPointAt(PointsCount + i - m_cutHollow * 2, Pos);
        }
        //
        m_points = Points.ToArray();
        //
        SetSplineGenerate();
    }

    private void SetSplineGenerate()
    {
        for (int i = 0; i < m_spline.GetPointCount(); i++)
        {
            m_spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            int PrevIndex = i == 0 ? (m_points.Length - 1) : (i - 1);
            int NextIndex = i == (m_points.Length - 1) ? 0 : (i + 1);
            Vector3 PrevPos = m_points[PrevIndex];
            Vector3 NextPos = m_points[NextIndex];
            SplineUtility.CalculateTangents(m_spline.GetPosition(i), PrevPos, NextPos, m_spriteShapeControllerTransform.forward, 0.71f, out Vector3 rightTangent, out Vector3 leftTangent);
            m_spline.SetLeftTangent(i, leftTangent);
            m_spline.SetRightTangent(i, rightTangent);
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(RendererCircumShape))]
public class ShapeCreatorEditor : Editor
{
    private RendererCircumShape m_target;

    private SerializedProperty m_spriteShape;

    private SerializedProperty FilledRadius;
    private SerializedProperty FilledCut;

    private SerializedProperty HollowRadius;
    private SerializedProperty HollowCut;

    private void OnEnable()
    {
        m_target = target as RendererCircumShape;
        //
        m_spriteShape = QEditorCustom.GetField(this, "m_spriteShape");
        //
        FilledRadius = QEditorCustom.GetField(this, "FilledRadius");
        FilledCut = QEditorCustom.GetField(this, "FilledCut");
        //
        HollowRadius = QEditorCustom.GetField(this, "HollowRadius");
        HollowCut = QEditorCustom.GetField(this, "HollowCut");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditorCustom.SetField(m_spriteShape);
        //
        QEditorCustom.SetField(FilledRadius);
        //
        if (QEditor.SetButton("Generate Filled"))
            m_target.SetGenerateFilled();
        //
        QEditorCustom.SetField(FilledCut);
        //
        if (QEditor.SetButton("Generate Filled Open"))
            m_target.SetGenerateFilledOpen();
        //
        QEditorCustom.SetField(HollowRadius);
        QEditorCustom.SetField(HollowCut);
        //
        if (QEditor.SetButton("Generate Hollow"))
            m_target.SetGenerateHollow();
        //
        QEditorCustom.SetApply(this);
    }
}

#endif
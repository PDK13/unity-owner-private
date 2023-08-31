using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCreator : MonoBehaviour
{
    public List<Vector2> Points;

    [Min(3)] public int GeometryPoints = 3;
    [Min(0)] public float GeometryRadius = 2f;
    [Range(0, 360)] public float GeometryRotate = 0f;

    [Min(0)] public float HoldRadius = 0f;

    public void SetCreate()
    {
        if (Points.Count < 3)
            return;
        //
        if (GetComponent<MeshFilter>() == null)
            return;
        //
        GetComponent<MeshFilter>().mesh = QMesh.GetMesh(Points);
    }

    public void SetCreateGeometry()
    {
        Points = QGeometry.GetGeometry(GeometryPoints, GeometryRadius, GeometryRotate);
        //
        SetCreate();
    }

    public void SetCreateGeometryHold()
    {
        if (HoldRadius == 0 || HoldRadius >= GeometryRadius)
        {
            SetCreateGeometry();
            return;
        }
        //
        List<Vector2> PointOutside = QGeometry.GetGeometry(GeometryPoints, GeometryRadius, GeometryRotate);
        List<Vector2> PointInside = QGeometry.GetGeometry(GeometryPoints, HoldRadius, GeometryRotate);
        //
        Points = new List<Vector2>();
        for (int i = 0; i < GeometryPoints; i++)
        {
            Points.Add(PointOutside[i]);
            Points.Add(PointInside[i]);
        }
        //
        SetCreate();
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MeshCreator))]
public class MeshCreatorEditor : Editor
{
    private MeshCreator m_target;

    private SerializedProperty Points;

    private SerializedProperty GeometryPoints;
    private SerializedProperty GeometryRadius;
    private SerializedProperty GeometryRotate;

    private SerializedProperty HoldRadius;

    private void OnEnable()
    {
        m_target = target as MeshCreator;
        //
        Points = QEditorCustom.GetField(this, "Points");
        //
        GeometryPoints = QEditorCustom.GetField(this, "GeometryPoints");
        GeometryRadius = QEditorCustom.GetField(this, "GeometryRadius");
        GeometryRotate = QEditorCustom.GetField(this, "GeometryRotate");
        //
        HoldRadius = QEditorCustom.GetField(this, "HoldRadius");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditorCustom.SetField(Points);
        //
        if (QEditor.SetButton("Generate"))
            m_target.SetCreate();
        //
        QEditorCustom.SetField(GeometryPoints);
        QEditorCustom.SetField(GeometryRadius);
        QEditorCustom.SetField(GeometryRotate);
        //
        if (QEditor.SetButton("Generate Geometry"))
            m_target.SetCreateGeometry();
        //
        QEditorCustom.SetField(HoldRadius);
        //
        if (QEditor.SetButton("Generate Geometry Hold"))
            m_target.SetCreateGeometryHold();
        QEditorCustom.SetApply(this);
    }
}

#endif
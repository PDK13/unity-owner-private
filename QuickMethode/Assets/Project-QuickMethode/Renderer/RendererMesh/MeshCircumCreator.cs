using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCircumCreator : MonoBehaviour
{
    [SerializeField] private MeshFilter m_meshFilter;

    [Space]
    [Min(3)] public int FilledPoints = 3;

    [Min(0)] public float FilledRadius = 2f;
    public float FilledDeg = 0f;

    [Space]
    [Min(0f)] public float[] FilledPointsRatio;

    [Space]
    [Min(0)] public float HollowRadius = 0f;

    [Space]
    public Vector3[] Points;
    public int[] Triangles;

    [Space]
    public MeshCircumCreatorData Circum;

    public void SetGenerate()
    {
        if (Application.isPlaying)
        {
            m_meshFilter.mesh.Clear();
            m_meshFilter.mesh.vertices = Points;
            m_meshFilter.mesh.triangles = Triangles;
            m_meshFilter.mesh.RecalculateNormals();
            m_meshFilter.mesh.RecalculateBounds();
        }
        else
        {
            Mesh Mesh = new Mesh();
            Mesh.vertices = Points;
            Mesh.triangles = Triangles;
            Mesh.RecalculateNormals();
            Mesh.RecalculateBounds();
            m_meshFilter.mesh = Mesh;
        }
    }

    public void SetGenerateFilled()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new MeshCircumCreatorData(m_meshFilter);
        //
        Circum.SetFilledGenerate(FilledPoints, FilledRadius, FilledDeg);
        //
        Points = Circum.Points;
        Triangles = Circum.Triangles;
    }

    public void SetGenerateFilledRatio()
    {
        if (FilledPointsRatio.Length < 3)
            return;
        //
        if (Circum == null || !Application.isPlaying)
            Circum = new MeshCircumCreatorData(m_meshFilter);
        //
        Circum.SetFilledGenerate(FilledPointsRatio, FilledRadius, FilledDeg);
        //
        Points = Circum.Points;
        Triangles = Circum.Triangles;
    }

    public void SetGenerateHollow()
    {
        if (Circum == null || !Application.isPlaying)
            Circum = new MeshCircumCreatorData(m_meshFilter);
        //
        Circum.SetHollowGenerate(FilledPoints, FilledRadius, HollowRadius, FilledDeg);
        //
        Points = Circum.Points;
        Triangles = Circum.Triangles;
    }
}

[Serializable]
public class MeshCircumCreatorData
{
    public int Point { private set; get; } = 0;
    public float Radius { private set; get; } = 0;
    public float Deg { private set; get; } = 0;

    public bool Hollow { private set; get; } = false;
    public float RadiusHollow { private set; get; } = 0;

    public Vector3[] Points { private set; get; } = new Vector3[0];
    public float[] PointsRatio { private set; get; } = new float[0];
    public int[] Triangles { private set; get; } = new int[0];

    private MeshFilter m_meshFilter;

    public MeshCircumCreatorData(MeshFilter MeshFilter)
    {
        m_meshFilter = MeshFilter;
    }

    //Filled - No Ratio

    public void SetFilledGenerate(int Point, float Radius, float Deg)
    {
        if (Point < 3)
            //One shape must have 3 points at least!!
            return;
        //
        this.Point = Point;
        this.PointsRatio = new float[0];
        this.Radius = Radius;
        this.Deg = Deg;
        this.Hollow = false;
        this.RadiusHollow = 0;
        //
        this.Points = GetFilledPoints();
        this.Triangles = GetFilledTriangle();
        //
        SetMeshFilter();
    }

    private Vector3[] GetFilledPoints()
    {
        if (Point < 3)
            //One shape must have 3 points at least!!
            return null;
        //
        List<Vector3> Points = new List<Vector3>();
        //
        float RadSpace = (360 / Point) * (Mathf.PI / 180);
        float RadStart = (Deg) * (Mathf.PI / 180);
        float RadCur = RadStart;
        //
        Vector3 PointStart = new Vector3(Mathf.Cos(RadStart) * Radius, Mathf.Sin(RadStart) * Radius, 0f);
        Points.Add(PointStart);
        for (int i = 1; i < Point; i++)
        {
            RadCur += RadSpace;
            Vector3 NewPoint = new Vector3(Mathf.Cos(RadCur) * Radius, Mathf.Sin(RadCur) * Radius, 0f);
            Points.Add(NewPoint);
        }
        //
        return Points.ToArray();
    }

    private int[] GetFilledTriangle()
    {
        int TriangleCount = Points.Length - 2;
        //
        List<int> Trianges = new List<int>();
        for (int i = 0; i < TriangleCount; i++)
        {
            Trianges.Add(0);
            Trianges.Add(i + 2);
            Trianges.Add(i + 1);
        }
        //
        return Trianges.ToArray();
    }

    //Filled - With Ratio

    public void SetFilledGenerate(float[] PointRatio, float Radius, float Deg)
    {
        if (PointRatio.Length < 3)
            //One shape must have 3 points at least!!
            return;
        //
        this.Point = PointRatio.Length;
        this.PointsRatio = PointRatio;
        this.Radius = Radius;
        this.Deg = Deg;
        this.Hollow = false;
        this.RadiusHollow = 0;
        //
        this.Points = GetFilledPoints();
        this.Points = GetFilledPointRatio();
        this.Triangles = GetFilledTriangle();
        //
        SetMeshFilter();
    }

    private Vector3[] GetFilledPointRatio()
    {
        Vector3[] Points = this.Points;
        for (int i = 0; i < PointsRatio.Length; i++)
            Points[i] = Points[i] + Points[i].normalized * PointsRatio[i];
        return Points;
    }

    //Hollow

    public void SetHollowGenerate(int Point, float Radius, float RadiusHollow, float Deg)
    {
        if (Point < 3)
            //One shape must have 3 points at least!!
            return;
        //
        this.Point = Point;
        this.PointsRatio = new float[0];
        this.Radius = Radius;
        this.Deg = Deg;
        this.Hollow = true;
        this.RadiusHollow = RadiusHollow;
        //
        this.Points = GetHollowPoints();
        this.Triangles = GetHollowTriangle();
        //
        SetMeshFilter();
    }

    private Vector3[] GetHollowPoints()
    {
        List<Vector3> Points = new List<Vector3>();
        //
        Points.AddRange(GetFilledPoints());
        Points.AddRange(GetFilledPoints());
        //
        return Points.ToArray();
    }

    private int[] GetHollowTriangle()
    {
        int PointCount = Points.Length / 2;
        int TriangleCount = PointCount * 2;
        //
        List<int> Trianges = new List<int>();
        for (int i = 0; i < PointCount; i++)
        {
            int OuterIndex = i;
            int InnerIndex = i + PointCount;
            //
            //First Triangle Start at Outer Edgle i
            Trianges.Add(OuterIndex);
            Trianges.Add(InnerIndex);
            Trianges.Add((i + 1) % PointCount);
            //
            //Second Triangle Start at Outer Edgle i
            Trianges.Add(OuterIndex);
            Trianges.Add(PointCount + ((PointCount + i - 1) % PointCount));
            Trianges.Add(OuterIndex + PointCount);
        }
        //
        return Trianges.ToArray();
    }

    //Mesh Filter

    private void SetMeshFilter()
    {
        if (m_meshFilter == null)
            return;
        //
        if (Application.isPlaying)
        {
            Mesh Mesh = new Mesh();
            Mesh.name = string.Format("{0}-{1}-{2}", Point, Radius + (Hollow ? "H" + RadiusHollow : "F"), Deg);
            Mesh.vertices = Points;
            Mesh.triangles = Triangles;
            Mesh.RecalculateNormals();
            Mesh.RecalculateBounds();
            m_meshFilter.mesh = Mesh;
        }
        else
        if (m_meshFilter.mesh != null)
        {
            m_meshFilter.mesh.Clear();
            m_meshFilter.mesh.vertices = Points;
            m_meshFilter.mesh.triangles = Triangles;
            m_meshFilter.mesh.RecalculateNormals();
            m_meshFilter.mesh.RecalculateBounds();
        }
        else
        if (m_meshFilter.sharedMesh != null)
        {
            m_meshFilter.sharedMesh.Clear();
            m_meshFilter.sharedMesh.vertices = Points;
            m_meshFilter.sharedMesh.triangles = Triangles;
            m_meshFilter.sharedMesh.RecalculateNormals();
            m_meshFilter.sharedMesh.RecalculateBounds();
        }
    }

    //Static

    public static List<Vector2> GetGeometryPoints(int Point, float Radius, float Deg)
    {
        if (Point < 3)
            //One shape must have 3 points at least!!
            return null;

        List<Vector2> Points = new List<Vector2>();

        float RadSpace = (360 / Point) * (Mathf.PI / 180);
        float RadStart = (Deg) * (Mathf.PI / 180);
        float RadCur = RadStart;

        Vector2 PointStart = new Vector2(Mathf.Cos(RadStart) * Radius, Mathf.Sin(RadStart) * Radius);
        Points.Add(PointStart);
        for (int i = 1; i < Point; i++)
        {
            RadCur += RadSpace;
            Vector2 NewPoint = new Vector2(Mathf.Cos(RadCur) * Radius, Mathf.Sin(RadCur) * Radius);
            Points.Add(NewPoint);
        }

        return Points;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MeshCircumCreator))]
public class MeshCreatorEditor : Editor
{
    private MeshCircumCreator m_target;

    private SerializedProperty m_meshFilter;

    private SerializedProperty FilledPoints;
    private SerializedProperty FilledRadius;
    private SerializedProperty FilledDeg;

    private SerializedProperty FilledPointsRatio;

    private SerializedProperty HollowRadius;

    private SerializedProperty Points;
    private SerializedProperty Triangles;

    private void OnEnable()
    {
        m_target = target as MeshCircumCreator;
        //
        m_meshFilter = QEditorCustom.GetField(this, "m_meshFilter");
        //
        FilledPoints = QEditorCustom.GetField(this, "FilledPoints");
        FilledRadius = QEditorCustom.GetField(this, "FilledRadius");
        FilledDeg = QEditorCustom.GetField(this, "FilledDeg");
        //
        FilledPointsRatio = QEditorCustom.GetField(this, "FilledPointsRatio");
        //
        HollowRadius = QEditorCustom.GetField(this, "HollowRadius");
        //
        Points = QEditorCustom.GetField(this, "Points");
        Triangles = QEditorCustom.GetField(this, "Triangles");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditorCustom.SetField(m_meshFilter);
        //
        QEditorCustom.SetField(FilledPoints);
        QEditorCustom.SetField(FilledRadius);
        QEditorCustom.SetField(FilledDeg);
        //
        if (QEditor.SetButton("Generate Filled"))
            m_target.SetGenerateFilled();
        //
        QEditorCustom.SetField(FilledPointsRatio);
        //
        if (QEditor.SetButton("Generate Filled Ratio"))
            m_target.SetGenerateFilledRatio();
        //
        QEditorCustom.SetField(HollowRadius);
        //
        if (QEditor.SetButton("Generate Hollow"))
            m_target.SetGenerateHollow();
        //
        QEditorCustom.SetField(Points);
        QEditorCustom.SetField(Triangles);
        //
        if (QEditor.SetButton("Generate"))
            m_target.SetGenerate();
        //
        QEditorCustom.SetApply(this);
    }
}

#endif
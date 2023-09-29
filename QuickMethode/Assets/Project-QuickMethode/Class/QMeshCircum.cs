using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class QMeshCircum
{
    #region Local

    private int m_point = 0;
    private float m_radius = 0;
    private float m_deg = 0;

    private bool m_hollow = false;
    private float m_radiusHollow = 0;

    private Vector3[] m_points;
    private int[] m_triangle;

    private Mesh m_mesh;

    public int Point => m_point;

    public float Radius => m_radius;

    public float Deg => m_deg;

    public bool Hollow => m_hollow;

    public float RadiusHollow => Hollow ? m_radiusHollow : 0f;

    public Vector3[] Points => m_points;

    public int[] Triangles => m_triangle;

    public Mesh Mesh => m_mesh;

    //Filled

    public Mesh SetFilledGenerate(int Point, float Radius, float Deg)
    {
        if (Point < 3)
            return null;
        //
        m_point = Point;
        m_radius = Radius;
        m_deg = Deg;
        m_hollow = false;
        m_radiusHollow = 0;
        //
        m_points = GetFilledPoints(Point, Radius, Deg).ToArray();
        m_triangle = GetFilledTriangle(m_points);
        //
        m_mesh = new Mesh();
        m_mesh.name = string.Format("{0}-{1}-{2}", m_point, m_radius + (m_hollow ? "H" + m_radiusHollow : "F"), m_deg);
        m_mesh.vertices = m_points;
        m_mesh.triangles = m_triangle;
        m_mesh.RecalculateNormals();
        m_mesh.RecalculateBounds();
        //
        return m_mesh;
    }

    private List<Vector3> GetFilledPoints(int Point, float Radius, float Deg)
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
        return Points;
    }

    private int[] GetFilledTriangle(Vector3[] Points)
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

    //Hollow

    public Mesh SetHollowGenerate(int Point, float Radius, float RadiusHollow, float Deg)
    {
        if (m_point < 3)
            return null;
        //
        m_point = Point;
        m_radius = Radius;
        m_deg = Deg;
        m_hollow = true;
        m_radiusHollow = RadiusHollow;
        //
        m_points = GetHollowPoints(m_point, m_radius, m_radiusHollow, m_deg).ToArray();
        m_triangle = GetHollowTriangle(m_points);
        //
        m_mesh = new Mesh();
        m_mesh.name = string.Format("{0}-{1}-{2}", m_point, m_radius + (m_hollow ? "H" + m_radiusHollow : "F"), m_deg);
        m_mesh.vertices = m_points;
        m_mesh.triangles = m_triangle;
        m_mesh.RecalculateNormals();
        m_mesh.RecalculateBounds();
        //
        return m_mesh;
    }

    private List<Vector3> GetHollowPoints(int Point, float Radius, float RadiusHollow, float Deg)
    {
        List<Vector3> Points = new List<Vector3>();
        //
        Points.AddRange(GetFilledPoints(Point, Radius, Deg));
        Points.AddRange(GetFilledPoints(Point, RadiusHollow, Deg));
        //
        return Points;
    }

    private int[] GetHollowTriangle(Vector3[] Points)
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

    #endregion

    #region Static

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

    #endregion
}
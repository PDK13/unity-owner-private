using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RendererMesh : MonoBehaviour
{
    [SerializeField] private MeshFilter m_meshFilter;

    [SerializeField] private Vector3[] m_points;
    [SerializeField] private int[] m_triangles;

    private RendererMeshData m_data;

    public Vector3[] Points => m_points;

    public int[] Triangles => m_triangles;

    public RendererMeshData Data => m_data;

    public void SetGenerate(bool AutoTriangle = true)
    {
        if (AutoTriangle)
            m_data = new RendererMeshData(m_points);
        else
            m_data = new RendererMeshData(m_points, m_triangles);
        //
        if (m_meshFilter == null)
            return;
        //
        if (Application.isPlaying)
        {
            Mesh Mesh = new Mesh();
            Mesh.name = this.name;
            Mesh.vertices = m_data.Points;
            Mesh.triangles = m_data.Triangles;
            Mesh.RecalculateNormals();
            Mesh.RecalculateBounds();
            m_meshFilter.mesh = Mesh;
        }
        else
        if (m_meshFilter.mesh != null)
        {
            m_meshFilter.mesh.Clear();
            m_meshFilter.mesh.vertices = m_data.Points;
            m_meshFilter.mesh.triangles = m_data.Triangles;
            m_meshFilter.mesh.RecalculateNormals();
            m_meshFilter.mesh.RecalculateBounds();
        }
        else
        if (m_meshFilter.sharedMesh != null)
        {
            m_meshFilter.sharedMesh.Clear();
            m_meshFilter.sharedMesh.vertices = m_data.Points;
            m_meshFilter.sharedMesh.triangles = m_data.Triangles;
            m_meshFilter.sharedMesh.RecalculateNormals();
            m_meshFilter.sharedMesh.RecalculateBounds();
        }
        else
        {
            Mesh Mesh = new Mesh();
            Mesh.name = this.name;
            Mesh.vertices = m_data.Points;
            Mesh.triangles = m_data.Triangles;
            Mesh.RecalculateNormals();
            Mesh.RecalculateBounds();
            m_meshFilter.mesh = Mesh;
        }
        //
        if (AutoTriangle)
            m_triangles = m_data.Triangles;
    }
}

public class RendererMeshData
{
    public Vector3[] Points { get; private set; } = new Vector3[0];
    public int[] Triangles { get; private set; } = new int[0];

    public RendererMeshData()
    {
        //...
    }

    public RendererMeshData(Vector3[] Points)
    {
        SetGenerate(Points);
    }

    public RendererMeshData(Vector3[] Points, int[] Triangles)
    {
        SetGenerate(Points, Triangles);
    }

    #region Mesh

    public bool SetGenerate(Vector3[] Points)
    {
        if (Points.Length < 3)
            //One shape must have 3 points at least!!
            return false;
        //
        this.Points = Points;
        this.Triangles = GetTriangles();
        //
        return true;
    }

    public bool SetGenerate(Vector3[] Points, int[] Triangles)
    {
        if (Points.Length < 3)
            //One shape must have 3 points at least!!
            return false;
        //
        this.Points = Points;
        this.Triangles = Triangles;
        //
        return true;
    }

    #endregion

    #region Mesh Auto Triangle

    //Beward: Don't fix those stuff!

    private int[] GetTriangles()
    {
        List<int> indices = new List<int>();

        int n = Points.Length;
        if (n < 3)
            return indices.ToArray();

        int[] V = new int[n];
        if (GetArea() > 0)
        {
            for (int v = 0; v < n; v++)
                V[v] = v;
        }
        else
        {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }

        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2;)
        {
            if ((count--) <= 0)
                return indices.ToArray();

            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;

            if (GetSnip(u, v, w, nv, V))
            {
                int a, b, c, s, t;
                a = V[u];
                b = V[v];
                c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);
                m++;
                for (s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }

        indices.Reverse();
        return indices.ToArray();
    }

    private float GetArea()
    {
        int n = Points.Length;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++)
        {
            Vector2 pval = Points[p];
            Vector2 qval = Points[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return (A * 0.5f);
    }

    private bool GetSnip(int u, int v, int w, int n, int[] V)
    {
        int p;
        Vector2 A = Points[V[u]];
        Vector2 B = Points[V[v]];
        Vector2 C = Points[V[w]];
        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;
        for (p = 0; p < n; p++)
        {
            if ((p == u) || (p == v) || (p == w))
                continue;
            Vector2 P = Points[V[p]];
            if (GetInsideTriangle(A, B, C, P))
                return false;
        }
        return true;
    }

    private bool GetInsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
        float cCROSSap, bCROSScp, aCROSSbp;

        ax = C.x - B.x; ay = C.y - B.y;
        bx = A.x - C.x; by = A.y - C.y;
        cx = B.x - A.x; cy = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;

        aCROSSbp = ax * bpy - ay * bpx;
        cCROSSap = cx * apy - cy * apx;
        bCROSScp = bx * cpy - by * cpx;

        return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
    }

    #endregion
}

#if UNITY_EDITOR

[CustomEditor(typeof(RendererMesh))]
public class RendererMeshEditor : Editor
{
    private RendererMesh m_target;

    private SerializedProperty m_meshFilter;

    private SerializedProperty m_points;
    private SerializedProperty m_triangles;

    private void OnEnable()
    {
        m_target = target as RendererMesh;
        //
        m_meshFilter = QUnityEditorCustom.GetField(this, "m_meshFilter");
        //
        m_points = QUnityEditorCustom.GetField(this, "m_points");
        m_triangles = QUnityEditorCustom.GetField(this, "m_triangles");
    }

    public override void OnInspectorGUI()
    {
        QUnityEditorCustom.SetUpdate(this);
        //
        QUnityEditorCustom.SetField(m_meshFilter);
        //
        QUnityEditorCustom.SetField(m_points);
        if (QUnityEditor.SetButton("Generate Auto Triangle"))
            m_target.SetGenerate();
        //
        QUnityEditorCustom.SetField(m_triangles);
        if (QUnityEditor.SetButton("Generate Custom Triangle"))
            m_target.SetGenerate(false);
        //
        QUnityEditorCustom.SetApply(this);
    }
}

#endif
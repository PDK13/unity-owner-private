using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RendererCircumMesh : MonoBehaviour
{
    [SerializeField] private MeshFilter m_meshFilter;

    //OUTER:
    [SerializeField][Range(3, 60)] private int m_outerPoint = 3;
    float[] m_outerPointRatio = new float[3];
    //
    [SerializeField][Min(0)] private float m_outerRadius = 2f;
    [SerializeField] private float m_outerDeg = 0f;

    //INTER:
    [Space]
    [SerializeField][Min(0)] private float m_interRadius = 2f;
    //
    private RendererCircumMeshData m_data;

    public int OuterPoint
    {
        get => m_outerPoint;
        set
        {
            if (value < 3)
                value = 3;
            //
            if (value > 60)
                value = 60;
            //
            m_outerPoint = value;
            m_outerPointRatio = new float[value];
        }
    }

    public float[] OuterPointRatio
    {
        get => m_outerPointRatio;
        set
        {
            if (value.Length < 3)
                return;
            //
            for (int i = 0; i < value.Length; i++) if (value[i] < 0 && value[i] < -OuterRadius) value[i] = -OuterRadius;
            //
            m_outerPoint = value.Length;
            m_outerPointRatio = value;
        }
    }

    public float OuterRadius
    {
        get => m_outerRadius;
        set
        {
            if (value < 0)
                value = 0;
            //
            if (value < InterRadius)
                InterRadius = value;
            //
            m_outerRadius = value;
        }
    }

    public float OuterDeg
    {
        get => m_outerDeg;
        set => m_outerDeg = value;
    }

    //

    public float InterRadius
    {
        get => m_interRadius;
        set
        {
            if (value > OuterRadius)
                OuterRadius = value;
            //
            m_interRadius = value;
        }
    }

    public RendererCircumMeshData Data => m_data;

    public void SetGenerate()
    {
        m_data = new RendererCircumMeshData(OuterPointRatio, OuterRadius, InterRadius, OuterDeg);
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
    }

    #region Sample

    public void SetSampleStarA()
    {
        OuterPoint = 10;
        //
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 2 != 0)
                continue;
            //
            OuterPointRatio[i] = OuterRadius * 0.6f;
        }
        //
        OuterDeg = -18f;
        Data.SetGenerate(m_outerPointRatio, m_outerRadius, m_interRadius, m_outerDeg);
    }

    public void SetSampleStarB()
    {
        OuterPoint = 12;
        //
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 4 == 0)
                OuterPointRatio[i] = OuterRadius * 0.3f;
            else
            if (i % 2 != 0)
                OuterPointRatio[i] = OuterRadius * 0.6f;
        }
        //
        OuterDeg = 30f;
        Data.SetGenerate(m_outerPointRatio, m_outerRadius, m_interRadius, m_outerDeg);
    }

    //

    public void SetSampleStarC()
    {
        OuterPoint = 15;
        //
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 != 0)
                continue;
            //
            OuterPointRatio[i] = OuterRadius * 0.5f;
        }
        //
        OuterDeg = -18f;
        Data.SetGenerate(m_outerPointRatio, m_outerRadius, m_interRadius, m_outerDeg);
    }

    public void SetSampleStarD()
    {
        OuterPoint = 18;
        //
        int Draw = 1;
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 == 0)
                OuterPointRatio[i] = OuterRadius * 0.4f;
            else
            {
                if (Draw == 1 || Draw == 2)
                    OuterPointRatio[i] = OuterRadius * 0.1f;
                Draw++;
                if (Draw > 4)
                    Draw = 1;
            }
        }
        //
        OuterDeg = 0f;
        Data.SetGenerate(m_outerPointRatio, m_outerRadius, m_interRadius, m_outerDeg);
    }

    //

    public void SetSampleStarE()
    {
        OuterPoint = 24;
        //
        int Draw = 1;
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 == 0)
                OuterPointRatio[i] = OuterRadius * 0.4f;
            else
            {
                if (Draw == 1 || Draw == 2)
                    OuterPointRatio[i] = OuterRadius * 0.2f;
                Draw++;
                if (Draw > 4)
                    Draw = 1;
            }
        }
        //
        OuterDeg = 22.5f;
        Data.SetGenerate(m_outerPointRatio, m_outerRadius, m_interRadius, m_outerDeg);
    }

    public void SetSampleStarF()
    {
        OuterPoint = 36;
        //
        int Draw = 1;
        for (int i = 0; i < OuterPoint; i++)
        {
            if (i % 3 == 0)
                OuterPointRatio[i] = OuterRadius * 0.6f;
            else
            {
                if (Draw == 1 || Draw == 2)
                    OuterPointRatio[i] = OuterRadius * 0.3f;
                Draw++;
                if (Draw > 4)
                    Draw = 1;
            }

        }
        //
        OuterDeg = 15f;
        Data.SetGenerate(m_outerPointRatio, m_outerRadius, m_interRadius, m_outerDeg);
    }

    #endregion

    #region Editor

    public void SetEditorPointsRatioChange()
    {
        if (m_outerPointRatio.Length < 3)
        {
            m_outerPoint = 3;
            m_outerPointRatio = new float[3];
        }
        else
        if (m_outerPointRatio.Length != m_outerPoint)
        {
            m_outerPointRatio = new float[m_outerPoint];
        }
    }

    private void OnDrawGizmosSelected()
    {
        QGizmos.SetWireSphere(this.transform.position, OuterRadius, Color.gray);
        QGizmos.SetWireSphere(this.transform.position, InterRadius, Color.gray);
        //
        SetGenerate();
        //
        Data.SetGenerate(m_outerPointRatio, m_outerRadius, m_interRadius, m_outerDeg);
        //
        //OUTER:
        Vector3 PointA, PointB;
        for (int i = 0; i < Data.Point - 1; i++)
        {
            PointA = this.transform.position + Data.OuterPoints[i];
            PointB = this.transform.position + Data.OuterPoints[i + 1];
            QGizmos.SetLine(PointA, PointB, Color.green, 0.1f);
        }
        PointA = this.transform.position + Data.OuterPoints[Data.OuterPoints.Length - 1];
        PointB = this.transform.position + Data.OuterPoints[0];
        QGizmos.SetLine(PointA, PointB, Color.green, 0.1f);
        //
        //INTER:
        for (int i = 0; i < Data.Point - 1; i++)
        {
            PointA = this.transform.position + Data.InterPoints[i];
            PointB = this.transform.position + Data.InterPoints[i + 1];
            QGizmos.SetLine(PointA, PointB, Color.green, 0.1f);
        }
        PointA = this.transform.position + Data.InterPoints[Data.InterPoints.Length - 1];
        PointB = this.transform.position + Data.InterPoints[0];
        QGizmos.SetLine(PointA, PointB, Color.green, 0.1f);
    }

    #endregion
}

public class RendererCircumMeshData
{
    //OUTER:

    public int Point { private set; get; } = 0;
    public float OuterRadius { private set; get; } = 0;
    public float Deg { private set; get; } = 0;

    public Vector3[] OuterPoints { private set; get; } = new Vector3[0];
    public float[] PointsRatio { private set; get; } = new float[0];

    //INTER:

    public float InterRadius { private set; get; } = 0;

    public Vector3[] InterPoints { private set; get; } = new Vector3[0];

    //MESH:

    public Vector3[] Points { private set; get; } = new Vector3[0];

    public int[] Triangles { private set; get; } = new int[0];

    public RendererCircumMeshData()
    {
        //...
    }

    public RendererCircumMeshData(int Point, float OuterRadius, float InterRadius, float Deg)
    {
        SetGenerate(Point, OuterRadius, InterRadius, Deg);
    }

    public RendererCircumMeshData(float[] PointRatio, float OuterRadius, float InterRadius, float Deg)
    {
        SetGenerate(PointRatio, OuterRadius, InterRadius, Deg);
    }

    //

    public bool SetGenerate(int Point, float OuterRadius, float InterRadius, float Deg)
    {
        if (Point < 3)
            //One shape must have 3 points at least!!
            return false;
        //
        this.Point = Point;
        this.PointsRatio = new float[0];
        this.OuterRadius = OuterRadius;
        this.InterRadius = InterRadius;
        this.Deg = Deg;
        //
        this.OuterPoints = GetInterterPoint(OuterRadius);
        this.InterPoints = GetInterterPoint(InterRadius);
        this.OuterPoints = GetPointRatio(OuterPoints, PointsRatio);
        this.InterPoints = GetPointRatio(InterPoints, PointsRatio);
        //
        List<Vector3> Points = new List<Vector3>();
        Points.AddRange(OuterPoints);
        Points.AddRange(InterPoints);
        this.Points = Points.ToArray();
        //
        this.Triangles = GetFilledTriangle();
        //
        return true;
    }

    public bool SetGenerate(float[] PointRatio, float OuterRadius, float InterRadius, float Deg)
    {
        if (PointRatio.Length < 3)
            //One shape must have 3 points at least!!
            return false;
        //
        this.Point = PointRatio.Length;
        this.PointsRatio = PointRatio;
        this.OuterRadius = OuterRadius;
        this.InterRadius = InterRadius;
        this.Deg = Deg;
        //
        this.OuterPoints = GetInterterPoint(OuterRadius);
        this.InterPoints = GetInterterPoint(InterRadius);
        this.OuterPoints = GetPointRatio(OuterPoints, PointsRatio);
        this.InterPoints = GetPointRatio(InterPoints, PointsRatio);
        //
        List<Vector3> Points = new List<Vector3>();
        Points.AddRange(OuterPoints);
        Points.AddRange(InterPoints);
        this.Points = Points.ToArray();
        //
        this.Triangles = GetHollowTriangle();
        //
        return true;
    }

    private Vector3[] GetInterterPoint(float Radius)
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

    private Vector3[] GetPointRatio(Vector3[] Points, float[] PointsRatio)
    {
        for (int i = 0; i < PointsRatio.Length; i++)
            Points[i] = Points[i] + Points[i].normalized * PointsRatio[i];
        return Points;
    }

    //MESH:

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
}

#if UNITY_EDITOR

[CustomEditor(typeof(RendererCircumMesh))]
public class RendererCircumMeshEditor : Editor
{
    private RendererCircumMesh m_target;

    private SerializedProperty m_meshFilter;

    private SerializedProperty m_outerPoint;
    private SerializedProperty m_outerRadius;
    private SerializedProperty m_outerDeg;
    //
    private SerializedProperty m_interPoint;
    private SerializedProperty m_interRadius;

    private Vector2 m_scrollOuterPointRatio;
    private Vector2 m_scrollInterPointRatio;

    private void OnEnable()
    {
        m_target = target as RendererCircumMesh;
        //
        m_meshFilter = QEditorCustom.GetField(this, "m_meshFilter");
        //
        m_outerPoint = QEditorCustom.GetField(this, "m_outerPoint");
        m_outerRadius = QEditorCustom.GetField(this, "m_outerRadius");
        m_outerDeg = QEditorCustom.GetField(this, "m_outerDeg");
        //
        m_interPoint = QEditorCustom.GetField(this, "m_interPoint");
        m_interRadius = QEditorCustom.GetField(this, "m_interRadius");
    }

    public override void OnInspectorGUI()
    {
        QEditorCustom.SetUpdate(this);
        //
        QEditorCustom.SetField(m_meshFilter);
        //
        QEditor.SetSpace(10);
        //
        QEditor.SetLabel("SETTING", QEditor.GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        //OUTER:
        QEditorCustom.SetField(m_outerPoint);
        //
        if (m_target.OuterRadius < m_target.InterRadius)
            m_target.InterRadius = m_target.OuterRadius;
        else
        if (m_target.InterRadius > m_target.OuterRadius)
            m_target.OuterRadius = m_target.InterRadius;
        //
        if (m_target.OuterPointRatio.Length != m_target.OuterPoint)
            m_target.OuterPointRatio = new float[m_target.OuterPoint];
        //
        int i = 0;
        m_scrollOuterPointRatio = QEditor.SetScrollViewBegin(m_scrollOuterPointRatio, QEditor.GetGUIHeight(105));
        while (i < m_target.OuterPoint)
        {
            if (m_target.OuterPointRatio[i] < 0 && m_target.OuterPointRatio[i] < -m_target.OuterRadius)
                m_target.OuterPointRatio[i] = -m_target.OuterRadius;
            //
            //VIEW:
            QEditor.SetHorizontalBegin();
            QEditor.SetLabel(string.Format("{0}", i), QEditor.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter), QEditor.GetGUIWidth(25));
            m_target.OuterPointRatio[i] = QEditor.SetField(m_target.OuterPointRatio[i], null, QEditor.GetGUIWidth(50));
            //
            if (m_target.Data != null)
                if (m_target.Data.OuterPoints != null)
                    if (i < m_target.Data.OuterPoints.Length)
                        QEditor.SetLabel(((Vector2)m_target.Data.OuterPoints[i]).ToString(), QEditor.GetGUILabel(FontStyle.Normal, TextAnchor.MiddleCenter));
            //
            QEditor.SetHorizontalEnd();
            //VIEW:
            //
            i++;
        }
        QEditor.SetScrollViewEnd();
        //
        QEditorCustom.SetField(m_outerRadius);
        QEditorCustom.SetField(m_outerDeg);
        //
        //INTER:
        QEditorCustom.SetField(m_interRadius);
        //
        QEditor.SetSpace(10);
        //
        QEditor.SetLabel("SAMPLE", QEditor.GetGUILabel(FontStyle.Bold, TextAnchor.MiddleCenter));
        //
        QEditor.SetHorizontalBegin();
        if (QEditor.SetButton("Star A"))
            m_target.SetSampleStarA();
        if (QEditor.SetButton("Star B"))
            m_target.SetSampleStarB();
        QEditor.SetHorizontalEnd();
        //
        QEditor.SetHorizontalBegin();
        if (QEditor.SetButton("Star C"))
            m_target.SetSampleStarC();
        if (QEditor.SetButton("Star D"))
            m_target.SetSampleStarD();
        QEditor.SetHorizontalEnd();
        //
        QEditor.SetHorizontalBegin();
        if (QEditor.SetButton("Star E"))
            m_target.SetSampleStarE();
        if (QEditor.SetButton("Star F"))
            m_target.SetSampleStarF();
        QEditor.SetHorizontalEnd();
        //
        QEditorCustom.SetApply(this);
    }
}

#endif
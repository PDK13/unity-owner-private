using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCreator : MonoBehaviour
{
    [SerializeField] private MeshFilter m_meshFilter;

    private QCircum m_circum;

    [Space]
    [Min(3)] public int FilledPoints = 3;
    [Min(0)] public float FilledRadius = 2f;
    public float FilledDeg = 0f;

    [Space]
    [Min(0)] public float HollowRadius = 0f;

    [Space]
    public Vector3[] Points;
    public int[] Triangles;

    public void SetGenerate()
    {
        m_meshFilter.mesh.Clear();
        m_meshFilter.mesh.vertices = Points;
        m_meshFilter.mesh.triangles = Triangles;
        m_meshFilter.mesh.RecalculateNormals();
        m_meshFilter.mesh.RecalculateBounds();
    }

    public void SetGenerateFilled()
    {
        SetInit();
        m_circum.SetFilledGenerate();
        //
        Points = m_circum.Points;
        Triangles = m_circum.Triangles;
        //
        m_meshFilter.mesh = QMeshCircum.SetGenerate(m_circum);
    }

    public void SetGenerateHollow()
    {
        SetInit();
        m_circum.SetHollowGenerate();
        //
        Points = m_circum.Points;
        Triangles = m_circum.Triangles;
        //
        m_meshFilter.mesh = QMeshCircum.SetGenerate(m_circum);
    }

    private void SetInit()
    {
        if (m_circum == null)
            m_circum = new QCircum();
        //
        m_meshFilter.sharedMesh = new Mesh();
        //
        m_circum.Point = FilledPoints;
        m_circum.Radius = FilledRadius;
        m_circum.RadiusHollow = HollowRadius;
        m_circum.Deg = FilledDeg;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MeshCreator))]
public class MeshCreatorEditor : Editor
{
    private MeshCreator m_target;

    private SerializedProperty m_meshFilter;

    private SerializedProperty FilledPoints;
    private SerializedProperty FilledRadius;
    private SerializedProperty FilledDeg;

    private SerializedProperty HollowRadius;

    private SerializedProperty Points;
    private SerializedProperty Triangles;

    private void OnEnable()
    {
        m_target = target as MeshCreator;
        //
        m_meshFilter = QEditorCustom.GetField(this, "m_meshFilter");
        //
        FilledPoints = QEditorCustom.GetField(this, "FilledPoints");
        FilledRadius = QEditorCustom.GetField(this, "FilledRadius");
        FilledDeg = QEditorCustom.GetField(this, "FilledDeg");
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